using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using System.Security.Cryptography;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
	/// <summary>
	/// WeiXinInterface 的摘要说明
	/// </summary>
	public class WeiXinInterface : PageBase, IHttpHandler
	{

		private HttpRequest request;
		private HttpResponse response;

		////公众平台上开发者设置的token, appID, EncodingAESKey
		private static readonly string sToken = "wangkai1";
		private static readonly string sAppID = "wxbbad034c543e6c42";
		private static readonly string sEncodingAESKey = "IgkMCqMSKu3nTI6c8xPsBI3pCy55dSWVHgB2vhqedL9";
		//private static readonly string sToken = "QDG6eK";
		//private static readonly string sAppID = "wx5823bf96d3bd56c7";
		//private static readonly string sEncodingAESKey = "jWmYm7qr5nMoAUwZRjGtBxmz3KA1tkAj3ykkR6q2B2C";

		private static readonly string replyTemp = "<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{3}]]></Content></xml>";

		private static readonly string cacheKeyForH5 = "WeiXinInterface_cacheKeyForH5";
		private static readonly string h5linkTemp = "详细猛戳 http://car.h5.yiche.com/{0}/?WT.mc_id=nbxclpkey";

		public void ProcessRequest(HttpContext context)
		{
			response = context.Response;
			request = context.Request;
			CommonFunction.WriteInvokeLog("WeiXinReply", string.Format("[WeiXinInterface] url :{0} {1}", request.HttpMethod, request.Url));

			if (!string.IsNullOrEmpty(request.QueryString["echostr"]))
			{
				// 微信接口验证
				CheckWeiXinEchostr();
			}
			else
			{
				// 回复用户消息
				ReplyByUser();
			}
		}

		/// <summary>
		/// 微信设置接口验证
		/// </summary>
		private void CheckWeiXinEchostr()
		{
			#region 验证

			string signature = "";
			string timestamp = "";
			string nonce = "";
			string echostr = "";
			if (!string.IsNullOrEmpty(request.QueryString["echostr"]))
			{ echostr = request.QueryString["echostr"].ToString(); }
			response.Write(echostr);

			#endregion
		}

		/// <summary>
		/// 回复用户消息
		/// </summary>
		private void ReplyByUser()
		{
			response.ContentType = "Text/XML";
			// response.ContentType = "text/plain";

			WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

			string encrypt_msg = string.Empty;// request.Form["encrypt_msg"];
			string msg_signature = request.QueryString["msg_signature"];
			string timestamp = request.QueryString["timestamp"];
			string nonce = request.QueryString["nonce"];

			if (request.InputStream != null)
			{
				System.IO.Stream sream = request.InputStream;
				if (sream.Length > 0)
				{
					System.IO.StreamReader sr = new System.IO.StreamReader(sream);
					encrypt_msg = sr.ReadToEnd();
				}
			}

			//msg_signature = "942e23d49a68c6948ba13a219a5904db0a4d9146";
			//timestamp = "1442918546";
			//nonce = "873156753";
			//encrypt_msg = "<xml><ToUserName><![CDATA[gh_64287a59e95c]]></ToUserName><Encrypt><![CDATA[zB90d/RhnSXDCxh3wSoymoxpQnDNs9Kugw9YSjIyGMR7+WS7vXoT4wsO/8eOKVFYWJKXWmqzeZtNu5FymfO7UDf/iqgovowLnnn+0uxKV19as0T99qLaUWnN9bx9tSdq8iiZ7LDf85W4bZe8oqYTT3L7WQTZkAgdNBLQU4GoX3CM/9O36fEZkJrwdXKvv/Gagy9MIE2uIFE0tnjC99/Iqro6sh6+r392fyyGITq2EOn0xgMXrKjiEfwh1TiN/EDZec3ePWXMpO3ZPTyyHMK7tZ90pmOHn1c5Xv6pgrHECxWGl5kP5hzU9wHtPX0SZA/6CMSs8OO1fJgny2kQfjsPv9ZFC12kS327KCBhA6IWvq391wlZOAeXVm9dL2KWKODSXRCBo8QazqrhA4Z9fsycxnksHjdtahc0BpjGFE/GIpM=]]></Encrypt></xml>";

			if (!string.IsNullOrEmpty(encrypt_msg) && !string.IsNullOrEmpty(msg_signature)
				&& !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(nonce))
			{
				CommonFunction.WriteInvokeLog("WeiXinReply", string.Format("[WeiXinInterface] url :{0} Post encrypt_msg:{1}", request.Url, encrypt_msg));
				int ret = 0;
				string messageSor = "";
				ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, encrypt_msg, ref messageSor);
				CommonFunction.WriteInvokeLog("WeiXinReply", string.Format("[WeiXinInterface] messageSor:{0}", messageSor));
				if (ret == 0 && messageSor != "")
				{
					try
					{
						XmlDocument doc = new XmlDocument();
						doc.LoadXml(messageSor);

						XmlNode root = doc.FirstChild;
						string ToUserName = root["ToUserName"].InnerText;
						string FromUserName = root["FromUserName"].InnerText;
						string MsgType = root["MsgType"].InnerText;
						string Content = root["Content"].InnerText;
						// string Content = "朗逸";
						string replyContent = GetReplyByUserContent(Content);
						string replyStr = string.Format(replyTemp, FromUserName, ToUserName, timestamp, replyContent);
						string encryptreplyStr = ""; //xml格式的密文
						ret = wxcpt.EncryptMsg(replyStr, timestamp, nonce, ref encryptreplyStr);
						CommonFunction.WriteInvokeLog("WeiXinReply", string.Format("[WeiXinInterface] encryptreplyStr:{0}", encryptreplyStr));
						response.Write(encryptreplyStr);
					}
					catch (Exception ex)
					{
						CommonFunction.WriteInvokeLog("WeiXinReply", string.Format("[WeiXinInterface] 加密发送消息xml异常 :{0}", ex.ToString()));
					}
				}
				else
				{ CommonFunction.WriteInvokeLog("WeiXinReply", string.Format("[WeiXinInterface] url :{0} ret:{1}", request.Url, ret)); }
			}
			else { CommonFunction.WriteInvokeLog("WeiXinReply", "[WeiXinInterface] 参数不全"); }
		}

		/// <summary>
		/// 根据用户输入指定回复
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		private string GetReplyByUserContent(string content)
		{
			string temp = "默认回复";
			if (!string.IsNullOrEmpty(content))
			{
				// <车系名,allSpell>
				Dictionary<string, string> dicCacheForH5 = new Dictionary<string, string>();
				object cacheForH5 = CacheManager.GetCachedData(cacheKeyForH5);
				if (cacheForH5 != null)
				{
					dicCacheForH5 = (Dictionary<string, string>)cacheForH5;
				}
				else
				{
					List<int> listAllCsID = new SerialFourthStageBll().GetAllSerialInH5();
					if (listAllCsID.Count > 0)
					{
						string sql = @"select cs.cs_Id,cs.cs_Name,cs.cs_OtherName,cs.cs_EName
											,cs.cs_ShowName,cs.cs_seoname,cs.allSpell
											from dbo.Car_Serial cs 
											left join dbo.Car_Serial_30UV cs30 on cs.cs_id=cs30.cs_id
											where cs.isState=1
											order by cs30.UVCount desc";
						DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
						if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
						{
							foreach (DataRow dr in ds.Tables[0].Rows)
							{
								int csid = BitAuto.Utils.ConvertHelper.GetInteger(dr["cs_Id"]);
								if (listAllCsID.Contains(csid))
								{
									string allSpell = dr["allSpell"].ToString().Trim();

									string cs_Name = dr["cs_Name"].ToString().Trim();
									GenerateKeyToDic(cs_Name, allSpell, dicCacheForH5);
									//if (!dicCacheForH5.ContainsKey(cs_Name))
									//{ dicCacheForH5.Add(cs_Name, allSpell); }
									string cs_OtherName = dr["cs_OtherName"].ToString().Trim();
									string[] arrayOther = cs_OtherName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
									if (arrayOther.Length > 0)
									{
										foreach (string otherName in arrayOther)
										{
											GenerateKeyToDic(otherName.Trim(), allSpell, dicCacheForH5);
											//if (!dicCacheForH5.ContainsKey(otherName.Trim()))
											//{ dicCacheForH5.Add(otherName.Trim(), allSpell); }
										}
									}
									string cs_EName = dr["cs_EName"].ToString().Trim();
									GenerateKeyToDic(cs_EName, allSpell, dicCacheForH5);
									//if (!dicCacheForH5.ContainsKey(cs_EName))
									//{ dicCacheForH5.Add(cs_EName, allSpell); }
									string cs_ShowName = dr["cs_ShowName"].ToString().Trim();
									GenerateKeyToDic(cs_ShowName, allSpell, dicCacheForH5);
									//if (!dicCacheForH5.ContainsKey(cs_ShowName))
									//{ dicCacheForH5.Add(cs_ShowName, allSpell); }
									string cs_seoname = dr["cs_seoname"].ToString().Trim();
									GenerateKeyToDic(cs_seoname, allSpell, dicCacheForH5);
									//if (!dicCacheForH5.ContainsKey(cs_seoname))
									//{ dicCacheForH5.Add(cs_seoname, allSpell); }
									
								}
							}
						}
					}
					CacheManager.InsertCache(cacheKeyForH5, dicCacheForH5, 60 * 2);
				}
				if (dicCacheForH5.ContainsKey(content.Trim().ToLower()))
				{
					temp = string.Format(h5linkTemp, dicCacheForH5[content.Trim()]);
				}
			}
			return temp;
		}
		
		private void GenerateKeyToDic(string key, string value, Dictionary<string, string> dic)
		{
			if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !dic.ContainsKey(key.ToLower()))
			{
				dic.Add(key.ToLower(), value);
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}

	#region WXBizMsgCrypt

	//-40001 ： 签名验证错误
	//-40002 :  xml解析失败
	//-40003 :  sha加密生成签名失败
	//-40004 :  AESKey 非法
	//-40005 :  appid 校验错误
	//-40006 :  AES 加密失败
	//-40007 ： AES 解密失败
	//-40008 ： 解密后得到的buffer非法
	//-40009 :  base64加密异常
	//-40010 :  base64解密异常
	internal class WXBizMsgCrypt
	{
		string m_sToken;
		string m_sEncodingAESKey;
		string m_sAppID;
		enum WXBizMsgCryptErrorCode
		{
			WXBizMsgCrypt_OK = 0,
			WXBizMsgCrypt_ValidateSignature_Error = -40001,
			WXBizMsgCrypt_ParseXml_Error = -40002,
			WXBizMsgCrypt_ComputeSignature_Error = -40003,
			WXBizMsgCrypt_IllegalAesKey = -40004,
			WXBizMsgCrypt_ValidateAppid_Error = -40005,
			WXBizMsgCrypt_EncryptAES_Error = -40006,
			WXBizMsgCrypt_DecryptAES_Error = -40007,
			WXBizMsgCrypt_IllegalBuffer = -40008,
			WXBizMsgCrypt_EncodeBase64_Error = -40009,
			WXBizMsgCrypt_DecodeBase64_Error = -40010
		};

		//构造函数
		// @param sToken: 公众平台上，开发者设置的Token
		// @param sEncodingAESKey: 公众平台上，开发者设置的EncodingAESKey
		// @param sAppID: 公众帐号的appid
		public WXBizMsgCrypt(string sToken, string sEncodingAESKey, string sAppID)
		{
			m_sToken = sToken;
			m_sAppID = sAppID;
			m_sEncodingAESKey = sEncodingAESKey;
		}


		// 检验消息的真实性，并且获取解密后的明文
		// @param sMsgSignature: 签名串，对应URL参数的msg_signature
		// @param sTimeStamp: 时间戳，对应URL参数的timestamp
		// @param sNonce: 随机串，对应URL参数的nonce
		// @param sPostData: 密文，对应POST请求的数据
		// @param sMsg: 解密后的原文，当return返回0时有效
		// @return: 成功0，失败返回对应的错误码
		public int DecryptMsg(string sMsgSignature, string sTimeStamp, string sNonce, string sPostData, ref string sMsg)
		{
			if (m_sEncodingAESKey.Length != 43)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
			}
			XmlDocument doc = new XmlDocument();
			XmlNode root;
			string sEncryptMsg;
			try
			{
				doc.LoadXml(sPostData);
				root = doc.FirstChild;
				sEncryptMsg = root["Encrypt"].InnerText;
			}
			catch (Exception)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ParseXml_Error;
			}
			//verify signature
			int ret = 0;
			ret = VerifySignature(m_sToken, sTimeStamp, sNonce, sEncryptMsg, sMsgSignature);
			if (ret != 0)
				return ret;
			//decrypt
			string cpid = "";
			try
			{
				sMsg = Cryptography.AES_decrypt(sEncryptMsg, m_sEncodingAESKey, ref cpid);
			}
			catch (FormatException)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecodeBase64_Error;
			}
			catch (Exception)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecryptAES_Error;
			}
			if (cpid != m_sAppID)
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateAppid_Error;
			return 0;
		}

		//将企业号回复用户的消息加密打包
		// @param sReplyMsg: 企业号待回复用户的消息，xml格式的字符串
		// @param sTimeStamp: 时间戳，可以自己生成，也可以用URL参数的timestamp
		// @param sNonce: 随机串，可以自己生成，也可以用URL参数的nonce
		// @param sEncryptMsg: 加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串,
		//						当return返回0时有效
		// return：成功0，失败返回对应的错误码
		public int EncryptMsg(string sReplyMsg, string sTimeStamp, string sNonce, ref string sEncryptMsg)
		{
			if (m_sEncodingAESKey.Length != 43)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
			}
			string raw = "";
			try
			{
				raw = Cryptography.AES_encrypt(sReplyMsg, m_sEncodingAESKey, m_sAppID);
			}
			catch (Exception)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_EncryptAES_Error;
			}
			string MsgSigature = "";
			int ret = 0;
			ret = GenarateSinature(m_sToken, sTimeStamp, sNonce, raw, ref MsgSigature);
			if (0 != ret)
				return ret;
			sEncryptMsg = "";

			string EncryptLabelHead = "<Encrypt><![CDATA[";
			string EncryptLabelTail = "]]></Encrypt>";
			string MsgSigLabelHead = "<MsgSignature><![CDATA[";
			string MsgSigLabelTail = "]]></MsgSignature>";
			string TimeStampLabelHead = "<TimeStamp><![CDATA[";
			string TimeStampLabelTail = "]]></TimeStamp>";
			string NonceLabelHead = "<Nonce><![CDATA[";
			string NonceLabelTail = "]]></Nonce>";

			// modified by chengl 2015-9-22
			List<string> tempList = new List<string>(16);
			tempList.Add("<xml>");
			tempList.Add(EncryptLabelHead);
			tempList.Add(raw);
			tempList.Add(EncryptLabelTail);
			tempList.Add(MsgSigLabelHead);
			tempList.Add(MsgSigature);
			tempList.Add(MsgSigLabelTail);
			tempList.Add(TimeStampLabelHead);
			tempList.Add(sTimeStamp);
			tempList.Add(TimeStampLabelTail);
			tempList.Add(NonceLabelHead);
			tempList.Add(sNonce);
			tempList.Add(NonceLabelTail);
			tempList.Add("</xml>");
			sEncryptMsg = string.Join("", tempList.ToArray());

			//sEncryptMsg = sEncryptMsg + "<xml>" + EncryptLabelHead + raw + EncryptLabelTail;
			//sEncryptMsg = sEncryptMsg + MsgSigLabelHead + MsgSigature + MsgSigLabelTail;
			//sEncryptMsg = sEncryptMsg + TimeStampLabelHead + sTimeStamp + TimeStampLabelTail;
			//sEncryptMsg = sEncryptMsg + NonceLabelHead + sNonce + NonceLabelTail;
			//sEncryptMsg += "</xml>";
			return 0;
		}

		public class DictionarySort : System.Collections.IComparer
		{
			public int Compare(object oLeft, object oRight)
			{
				string sLeft = oLeft as string;
				string sRight = oRight as string;
				int iLeftLength = sLeft.Length;
				int iRightLength = sRight.Length;
				int index = 0;
				while (index < iLeftLength && index < iRightLength)
				{
					if (sLeft[index] < sRight[index])
						return -1;
					else if (sLeft[index] > sRight[index])
						return 1;
					else
						index++;
				}
				return iLeftLength - iRightLength;

			}
		}
		//Verify Signature
		private static int VerifySignature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, string sSigture)
		{
			string hash = "";
			int ret = 0;
			ret = GenarateSinature(sToken, sTimeStamp, sNonce, sMsgEncrypt, ref hash);
			if (ret != 0)
				return ret;
			//System.Console.WriteLine(hash);
			if (hash == sSigture)
				return 0;
			else
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateSignature_Error;
			}
		}

		public static int GenarateSinature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, ref string sMsgSignature)
		{
			ArrayList AL = new ArrayList();
			AL.Add(sToken);
			AL.Add(sTimeStamp);
			AL.Add(sNonce);
			AL.Add(sMsgEncrypt);
			AL.Sort(new DictionarySort());
			string raw = "";
			for (int i = 0; i < AL.Count; ++i)
			{
				raw += AL[i];
			}

			SHA1 sha;
			ASCIIEncoding enc;
			string hash = "";
			try
			{
				sha = new SHA1CryptoServiceProvider();
				enc = new ASCIIEncoding();
				byte[] dataToHash = enc.GetBytes(raw);
				byte[] dataHashed = sha.ComputeHash(dataToHash);
				hash = BitConverter.ToString(dataHashed).Replace("-", "");
				hash = hash.ToLower();
			}
			catch (Exception)
			{
				return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ComputeSignature_Error;
			}
			sMsgSignature = hash;
			return 0;
		}
	}

	#endregion

}