<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempBsToCs.aspx.cs" Inherits="BitAuto.CarChannelAPI.Web.Test.TempBsToCs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .table_style
        {
            background: #ccc;
            width: 100%;
        }
    </style>
</head>
<body>
    <div>
        <table width="100%" border="0" cellpadding="0" cellspacing="1" class="table_style">
            <tbody>
                <tr style="color: White; background-color: #507CD1; font-weight: bold;">
                    <th scope="col" width="5%" style="text-align: center; height: 28px;">主品牌ID
                    </th>
                    <th scope="col" width="10%" style="text-align: center; height: 28px;">主品牌名
                    </th>
                    <th scope="col" width="10%" style="text-align: center; height: 28px;">品牌ID
                    </th>
                    <th scope="col" width="15%" style="text-align: center; height: 28px;">品牌名
                    </th>
                    <th scope="col" width="10%" style="text-align: center; height: 28px;">子品牌ID
                    </th>
                    <th scope="col" width="15%" style="text-align: center; height: 28px;">子品牌名
                    </th>
                    <th scope="col" width="15%" style="text-align: center; height: 28px;">子品牌显示名
                    </th>
                </tr>
                <%=HtmlBsToCs %>
            </tbody>
        </table>
    </div>

</body>
</html>
