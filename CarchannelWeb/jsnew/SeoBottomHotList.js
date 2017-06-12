﻿function addLoadEvent(func) {
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

function addClass(element, value) {
    if (!element.className) {
        element.className = value;
    } else {
        newClassName = element.className;
        newClassName += " ";
        newClassName += value;
        element.className = newClassName;
    }
}

function removeClass(element, value) {
    var removedClass = element.className;
    var pattern = new RegExp("(^| )" + value + "( |$)");
    removedClass = removedClass.replace(pattern, "$1");
    removedClass = removedClass.replace(/ $/, "");
    element.className = removedClass;
    return true;
}
function all_func() {
    tabs("car_tab_ul", "data_table", "best_car", true);
}
addLoadEvent(all_func)

/*=======================tab=============================*/
function hide(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "none" } }
function show(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "block" } }

function tabsRemove(index, head, divs, div2s) {
    if (!document.getElementById(head)) return false;
    var tab_heads = document.getElementById(head);
    if (tab_heads) {
        var alis = tab_heads.getElementsByTagName("li");
        for (var i = 0; i < alis.length; i++) {
            removeClass(alis[i], "current");
            hide(divs + "_" + i);
            if (div2s) { hide(div2s + "_" + i) };
            if (i == index) {
                addClass(alis[i], "current");
            }
        }
        show(divs + "_" + index);
        if (div2s) { show(div2s + "_" + index) };
    }
}

function tabs(head, divs, div2s, over) {
    if (!document.getElementById(head)) return false;
    var tab_heads = document.getElementById(head);
    if (tab_heads) {
        var alis = tab_heads.getElementsByTagName("li");
        for (var i = 0; i < alis.length; i++) {
            alis[i].num = i;
            if (over) {
                alis[i].onmouseover = function() {
                    var thisobj = this;
                    thetabstime = setTimeout(function() { changetab(thisobj); }, 150);
                }
                alis[i].onmouseout = function() {
                    clearTimeout(thetabstime);
                }
            }
            else {
                alis[i].onclick = function() {
                    if (this.className == "current" || this.className == "last current") {
                        changetab(this);
                        return true;
                    }
                    else {
                        changetab(this);
                        return false;
                    }
                }
            }
            function changetab(thebox) {
                tabsRemove(thebox.num, head, divs, div2s);
            }
        }
    }
}