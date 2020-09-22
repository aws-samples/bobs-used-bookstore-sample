(function($) {
	"use strict"; 
	
	$(".dropdown-menu li a").on('click',function () {
		$(this).parents(".dropdown").find('.btn').html($(this).html() + ' <span class="caret"></span>');
		$(this).parents(".dropdown").find('.btn').val($(this).data('value'));
	});

	//Full_Screen
	$(".fullscreen-btn").on("click", function () {
		document.fullScreenElement && null !== document.fullScreenElement || !document.mozFullScreen && !document.webkitIsFullScreen ? document.documentElement.requestFullScreen ? document.documentElement.requestFullScreen() : document.documentElement.mozRequestFullScreen ? document.documentElement.mozRequestFullScreen() : document.documentElement.webkitRequestFullScreen && document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT) : document.cancelFullScreen ? document.cancelFullScreen() : document.mozCancelFullScreen ? document.mozCancelFullScreen() : document.webkitCancelFullScreen && document.webkitCancelFullScreen()
	});

	// collapse button in panel
	$(document).on('click', '.t-collapse', function () {
		var el = $(this).parents(".card").children(".card_chart");
		if ($(this).hasClass("fa-chevron-down")) {
			$(this).removeClass("fa-chevron-down").addClass("fa-chevron-up");
			el.slideUp(200);

		} else {
			$(this).removeClass("fa-chevron-up").addClass("fa-chevron-down");
			el.slideDown(200);

		}
	});

	//close button in panel
	$(document).on('click', '.t-close', function () {
		$(this).parents(".card, .stats-wrap").parent().remove();
	});

	//Scroll_BAr

	$(".scroll_auto").mCustomScrollbar({
		setWidth: false,
		setHeight: false,
		setTop: 0,
		setLeft: 0,
		axis: "y",
		scrollbarPosition: "inside",
		scrollInertia: 950,
		autoDraggerLength: true,
		autoHideScrollbar: false,
		autoExpandScrollbar: false,
		alwaysShowScrollbar: 0,
		snapAmount: null,
		snapOffset: 0
	});

	//Click_menu_icon_Add_Class_body
	$(".icon_menu").on('click', function () {
		if ($(window).width() > 767) {
			$('body').toggleClass("nav_small");
		} else {
			$('body').toggleClass("mobile_nav");
		}
	});

	// back-to-top
	$(window).on('scroll', function () {
		if ($(this).scrollTop() > 50) {
			$('#back-to-top').fadeIn();
		} else {
			$('#back-to-top').fadeOut();
		}
	});
	// scroll body to 0px on click
	$('#back-to-top').on('click', function () {

		$('body,html').animate({
			scrollTop: 0
		}, 800);
		return false;
	});

	//===ToolTip
	$(function () {
		$('[data-toggle="tooltip"]').tooltip();
	});

	//Add_li
	$(".todo--panel").on("submit", "form", function (a) {
		a.preventDefault();
		a = $(this);
		var c = a.find(".form-control");

		$('<li class="list-group-item" style="display: none;"><label class="todo--label"><input type="checkbox" name="" value="1" class="todo--input"><span class="todo--text">' + c.val() + '</span></label><a href="#" class="todo--remove">&times;</a></li>').appendTo(".list-group").slideDown("slow");
		c.val("");
	}).on("click", ".todo--remove", function (a) {
		a.preventDefault();
		var c = $(this).parent("li");
		c.slideUp("slow", function () {
			c.remove();
		});
	});
	$('#dc_accordion').dcAccordion();
	// End
})(jQuery);