var popWin = {
	scrolling: "auto",
	"int": function() {
		this.mouseClose(),
		this.closeMask()
	},
	
	mouseClose: function() {
		$("#popWinClose").on("mouseenter",
		function() {
			$(this).css("background-image", "url(./images/closehdtipper.png)")
		}),
		$("#popWinClose").on("mouseleave",
		function() {
			$(this).css("background-image", "url(./images/closehdtipper.png)")
		})
	},
	closeMask: function() {
		$("#popWinClose").on("click",
		function() {
			$("#mask,#maskTop").fadeOut(function() {
				$(this).remove()
			})
		})
	}
};
//���ض���
function b() {
	h = $(window).height(),
	t = $(document).scrollTop(),
	t > 300 ? $("#moquu_top").show() : $("#moquu_top").hide()
}
$(document).ready(function() {
	b(),
	$("#moquu_top").click(function() {
		$('html,body').animate({scrollTop: '0px'}, 800);
	})
}),
$(window).scroll(function() {
	b()
});