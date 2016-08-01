(function (doc, win) {
	          var docEl = doc.documentElement,
	            resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize',
	            recalc = function () {
	              var clientWidth = docEl.clientWidth;
	              if (!clientWidth) return;
	              docEl.style.fontSize = 20 * (clientWidth / 320) + 'px';
	            };
	
	          if (!doc.addEventListener) return;
	          win.addEventListener(resizeEvt, recalc, false);
	          doc.addEventListener('DOMContentLoaded', recalc, false);
	         })(document, window);
	         
$(function(){
	$(".leftarrow a").click(function(){
		window.history.go("-1");
	})
	$(".backhome a").click(function(){
		window.location.href="index.html";
	}	)
	
})
