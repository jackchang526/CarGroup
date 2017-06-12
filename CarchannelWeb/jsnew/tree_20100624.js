function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function() {
            oldonload();
            func();
        }
    }
}

function hasClass(ele, cls) {
    return ele.className.match(new RegExp('(\\s|^)' + cls + '(\\s|$)'));
}

function addClass(ele, cls) {
    if (!this.hasClass(ele, cls)) ele.className += " " + cls;
}

function removeClass(ele, cls) {
    if (hasClass(ele, cls)) {
        var reg = new RegExp('(\\s|^)' + cls + '(\\s|$)');
        ele.className = ele.className.replace(reg, ' ');
    }
}

/*className */
function getElementsByClass(searchClass, node, tag) {
    var classElements = new Array();
    if (node == null)
        node = document;
    if (tag == null)
        tag = '*';
    var els = node.getElementsByTagName(tag);
    var elsLen = els.length;
    var pattern = new RegExp("(^|\\s)" + searchClass + "(\\s|$)");
    for (i = 0, j = 0; i < elsLen; i++) {
        if (pattern.test(els[i].className)) {
            classElements[j] = els[i];
            j++;
        }
    }
    return classElements;
}

/*nextSibling*/
function get_nextSibling(n) {
    if (n && n.nextSibling) {
        var y = n.nextSibling;
        while (y.nodeType != 1) {
            y = y.nextSibling;
        }
        return y;
    }else
    {
    return null;
    }
}
/*firstChild*/
function get_firstChild(n) {
    var y = n.firstChild;
    while (y.nodeType != 1) {
        y = y.nextSibling;
    }
    return y;
}
/*lastChild*/
function get_lastChild(n) {
    var y = n.lastChild;
    while (y.nodeType != 1) {
        y = y.previousSibling;
    }
    return y;
}

/*previousSibling*/
function get_previousSibling(n) {
    var y = n.previousSibling;
    while (y.nodeType != 1) {
        y = y.previousSibling;
    }
    return y;
}

String.prototype.format = function() {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function(m, i) {
            return args[i];
        });
}

/*tree_box offsetheight*/
var tree_box = document.getElementById("tree_box");
var tree_box_height = tree_box.offsetHeight; // tree top box height
var nav_tree1 = document.getElementById("nav_tree");
//nav_tree.style.height = document.documentElement.clientHeight + nav_tree.offsetHeight - tree_box_height + "px";
if (nav_tree1 && tree_box_height) { nav_tree1.style.height = nav_tree1.offsetHeight - tree_box_height + "px"; }

/*tree*/
function nav_tree() {
    if (!document.getElementById("nav_tree")) return false;
    var nav_tree = document.getElementById("nav_tree");
    var tree_li = nav_tree.getElementsByTagName("li");
    var tree_a = nav_tree.getElementsByTagName("a");

    var pluses = nav_tree.getElementsByTagName("em");
    for (var i = 0; i < pluses.length; i++) {
        pluses[i].onmouseover = function() {
            this.parentNode.onclick = function()//使节点可点击
            {
                return false;
            }
            this.parentNode.setAttribute("selectme", "false");
        };
        pluses[i].onmouseout = function() {
            this.parentNode.onclick = null; //使节点不可点击
            this.parentNode.setAttribute("selectme", "true");
        };
    }

    for (var i = 0; i < tree_a.length; i++) {
        tree_a[i].setAttribute("orgurl", tree_a[i].href);
        tree_a[i].onmousedown = function() {
            var att = this.getAttribute("selectme");
            var selected = (att == null ? "true" : att) == "true";
            var cu = getElementsByClass("current", nav_tree, "a"); //remove current selected
            for (var i = 0; i < cu.length; i++) {
                removeClass(cu[i], "current");
            }
            if (hasClass(this, 'open')) {
                removeClass(this, "open");
                if (selected) addClass(this, "current");
                if (get_nextSibling(this)) { get_nextSibling(this).className = "none"; }
            }
            else if (this.className == "opened" || this.className == "last") {
                addClass(this, "current");
            }
            else {
                addClass(this, "open");
                if (selected) addClass(this, "current");
                if (get_nextSibling(this)) { get_nextSibling(this).className = ""; }
            }

            if (tree_app) {
                RedirectToCity(this);
            }
        }
    }
    if (typeof masterId == 'undefined' || typeof currentId == 'undefined') return;
    scroll_up(masterId, true);
    scroll_select(currentId);

}
addLoadEvent(nav_tree);

function tree_CloseAll() {
    var items = document.getElementsByName("master");
    for (var i = 0; i < items.length; i++) {
        items[i].className = "";
        if (get_nextSibling(items[i])) { get_nextSibling(items[i]).className = "none"; }
    }
}

/*tree letter*/
function tree_letter() {

    if (!document.getElementById("tree_letter")) return false;
    var tree_l = document.getElementById("tree_letter");
    var tree_l_a = tree_l.getElementsByTagName("a");

    /*var CharList={"A":"letter_A","B":"letter_B","C":"letter_C","D":"letter_D","F":"letter_F","G":"letter_G","H":"letter_H","J":"letter_J","K":"letter_K","L":"letter_L","M":"letter_M","O":"letter_O","P":"letter_P","Q":"letter_Q","R":"letter_R","S":"letter_S","T":"letter_T","W":"letter_W","X":"letter_X","Y":"letter_Y","Z":"letter_Z"};*/
    var nav_tree = document.getElementById("nav_tree");

    for (var i = 0; i < tree_l_a.length; i++) {
        tree_l_a[i].onclick = function() {
            var char = this.innerHTML;
            scroll_up(CharList[char]); //scroll up
            return false;

        }


    }

}

addLoadEvent(tree_letter);


/*scroll up*/
function scroll_up(masterID, openli) {
    if (!document.getElementById(masterID)) return false;
    var letter_a = document.getElementById(masterID);

    var scrollheight = letter_a.offsetTop; //a offsettop
    var scroll_num = scrollheight - tree_box_height; //different

    document.documentElement.scrollTop = scroll_num;

    if (document.documentElement.scrollTop != 0) {
        document.documentElement.scrollTop = scroll_num;
        //return false;
    }
    else {//chrome
        document.body.scrollTop = scroll_num;
        //window.pageYOffset = scroll_num + "px";
        //return false;
    }
    if (openli) {
        addClass(letter_a, "open");
        get_nextSibling(letter_a).className = "";
    }


}

/*scroll select current*/
function scroll_select(aID) {
    if (!document.getElementById(aID)) return false;
    var ID = document.getElementById(aID);
    var cu = getElementsByClass("current", null, "a");
    for (var i = 0; i < cu.length; i++) {
        removeClass(cu[i], "current");
    }
    addClass(ID, "current");
}

var UrlStore = {
    "photo": {
        "master": "http://photo.bitauto.com/master/{0}",
        "brand": "http://photo.bitauto.com/brand/{0}",
        "serial": "http://photo.bitauto.com/serial/{0}",
        "default": "http://photo.bitauto.com/index.html"
    },
    "price": {
        "master": "http://price.bitauto.com/frame.aspx?keyword={1}&mb_id={0}",
        "brand": "http://price.bitauto.com/frame.aspx?keyword={1}&b_id={0}",
        "serial": "http://price.bitauto.com/frame.aspx?newbrandid={0}",
        "default": "http://price.bitauto.com/"
    },
    "dealer": {
        "master": "http://dealer.bitauto.com/",
        "brand": "http://dealer.bitauto.com/{2}/",
        "serial": "http://dealer.bitauto.com/{2}/",
        "default": "http://dealer.bitauto.com"
    },
    "video": {
        "master": "http://v.bitauto.com/car/master/{0}.html",
        "brand": "http://v.bitauto.com/car/brand/{0}.html",
        "serial": "http://v.bitauto.com/car/serial/{0}.html",
        "default": "http://v.bitauto.com/car/index.aspx"
    }
};


function ChangeTab(tabName) {
    var nav_tree = document.getElementById("nav_tree");
    var selectedNode = getElementsByClass("current", nav_tree, "a");

    if (selectedNode < 1) {
        parent.window.location = UrlStore[tabName]["default"];
        return;
    }

    var id = selectedNode[0].id.substring(1);
    var text = selectedNode[0].title;
    var node = selectedNode[0].name;
    var spell = selectedNode[0].getAttribute("allspell");


    var url = UrlStore[tabName][node];
    url = url.format(id, encodeURIComponent(text), spell);

    var cityCookie = getCookie("bitauto_framecity");

    if (cityCookie) {
        var cityData = cityCookie.split(",");
        if (cityData.length == 2 && cityData[0] != 0) {
            if (tabName == "price") {
                url += "&citycode=" + cityData[0];
            }
            if (tabName == "dealer") {
                if (node != "master") {
                    url = "http://dealer.bitauto.com/{0}/{1}/";
                    url = url.format(cityData[1], spell);
                }
                else {
                    var brand = getFirstBrand(selectedNode[0]);
                    if (brand) {
                        var brandSpell = brand.getAttribute("allspell");
                        url = "http://dealer.bitauto.com/{0}/{1}/";
                        url = url.format(cityData[1], brandSpell);
                    }
                }
            }
        }
        else {
            if (tabName == "dealer") {
                if (node == "master") {
                    var brand = getFirstBrand(selectedNode[0]);
                    if (brand) {
                        var brandSpell = brand.getAttribute("allspell");
                        url = "http://dealer.bitauto.com/{0}/";
                        url = url.format(brandSpell);
                    }
                }
            }
        }
    }
    else {
        if (tabName == "dealer") {
            if (node == "master") {
                var brand = getFirstBrand(selectedNode[0]);
                if (brand) {
                    var brandSpell = brand.getAttribute("allspell");
                    url = "http://dealer.bitauto.com/{0}/";
                    url = url.format(brandSpell);
                }
            }
        }
    }
    //alert(url);
    if (parent) {
        parent.window.location = url.toLowerCase();
    }
}

//读取cookies函数
function getCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]); return null;
}

function getFirstBrand(sender) {
    if (sender.parentNode) {
        var brands = sender.parentNode.getElementsByTagName("UL");
        if (brands.length > 0) {
            var brand = brands[0].getElementsByTagName("A");
            if (brand && brand.length > 0) {
                return brand[0];
            }
        }
    }
    return null;
}

function RedirectToCity(sender) {
    var cityCookie = getCookie("bitauto_framecity");

    if (cityCookie) {
        var cityData = cityCookie.split(",");
        var orgUrl = sender.getAttribute("orgurl");
        var desUrl = orgUrl;
        sender.href = "javascript:void(0)";
        if (cityData.length == 2 && cityData[0] != 0) {
            if (tree_app == "price") {
                ///alert(orgUrl+"&citycode="+cityData[0]);
                desUrl = orgUrl + "&citycode=" + cityData[0];
            }
            if (tree_app == "dealer") {
                if (orgUrl.substr(0, "4") == "http") {
                    var paths = orgUrl.substr(7, orgUrl.length).split("/");
                    if (paths.length >= 2) {
                        //alert("http://"+paths[0]+"/"+cityData[1]+"/"+paths[1]+"/");
                        desUrl = "http://" + paths[0] + "/" + cityData[1] + "/" + paths[1] + "/";
                    }
                }
            }
        }
        sender.href = desUrl;
    }
}
