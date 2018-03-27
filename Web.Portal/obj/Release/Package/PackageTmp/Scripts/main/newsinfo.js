"use strict";

var winH = $(window).height();
var content = $(".content");
var allH = content.height() + 375;
if (allH < winH) {
  content.height(winH - 375);
}