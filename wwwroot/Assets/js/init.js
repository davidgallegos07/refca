$(document).ready(function(){
  // 1
  // Add smooth scrolling to all links in navbar + footer link
  $(".navbar a, footer a[href='#myPage']").on('click', function(event) {
    // Make sure this.hash has a value before overriding default behavior
    if (this.hash !== "") {
      // Prevent default anchor click behavior
      event.preventDefault();

      // Store hash
      var hash = this.hash;

      // Using jQuery's animate() method to add smooth page scroll
      // The optional number (900) specifies the number of milliseconds it takes to scroll to the specified area
      $('html, body').animate({
        scrollTop: $(hash).offset().top
      }, 900, function(){

        // Add hash (#) to URL when done scrolling (default click behavior)
        window.location.hash = hash;
      });
    } // End if
  });

  // 2
  $(window).scroll(function() {
    $(".slideanim").each(function(){
      var pos = $(this).offset().top;

      var winTop = $(window).scrollTop();
        if (pos < winTop + 600) {
          $(this).addClass("slide");
        }
    });
  });

  // 3
  $( document ).ready(function() {

      scaleVideoContainer();

      initBannerVideoSize('.video-container .poster img');
      initBannerVideoSize('.video-container .filter');
      initBannerVideoSize('.video-container video');

      $(window).on('resize', function() {
          scaleVideoContainer();
          scaleBannerVideoSize('.video-container .poster img');
          scaleBannerVideoSize('.video-container .filter');
          scaleBannerVideoSize('.video-container video');
      });

  });

  // 4
  function scaleVideoContainer() {

      var height = $(window).height() + 5;
      var unitHeight = parseInt(height) + 'px';
      $('.homepage-hero-module').css('height',unitHeight);

  }

  function initBannerVideoSize(element){

      $(element).each(function(){
          $(this).data('height', $(this).height());
          $(this).data('width', $(this).width());
      });

      scaleBannerVideoSize(element);

  }

  // 6
  function scaleBannerVideoSize(element){

      var windowWidth = $(window).width(),
      windowHeight = $(window).height() + 5,
      videoWidth,
      videoHeight;

      $(element).each(function(){
          var videoAspectRatio = $(this).data('height')/$(this).data('width');

          $(this).width(windowWidth);

          if(windowWidth < 1000){
              videoHeight = windowHeight;
              videoWidth = videoHeight / videoAspectRatio;
              $(this).css({'margin-top' : 0, 'margin-left' : -(videoWidth - windowWidth) / 2 + 'px'});

              $(this).width(videoWidth).height(videoHeight);
          }

          $('.homepage-hero-module .video-container video').addClass('fadeIn animated');

      });
  }

  // 7
  $(document).ready(function() {
    $('[data-toggle=offcanvas]').click(function() {
    $('.row-offcanvas').toggleClass('active');
    });
  });

  // 8
  $(function(){

    $('#tesis').click(function(e){
    	e.preventDefault();
        $('#mytabs a[href="#tesis_tab"]').tab('show');
    })

    $('#libro').click(function(e){
    	e.preventDefault();
        $('#mytabs a[href="#libro_tab"]').tab('show');
    })

    $('#cap').click(function(e){
      e.preventDefault();
        $('#mytabs a[href="#cap_tab"]').tab('show');
    })

    $('#investigacion').click(function(e){
      e.preventDefault();
        $('#mytabs a[href="#inv_tab"]').tab('show');
    })

    $('#arbitrados').click(function(e){
      e.preventDefault();
        $('#mytabs a[href="#arb_tab"]').tab('show');
    })

    $('#indexados').click(function(e){
      e.preventDefault();
        $('#mytabs a[href="#index_tab"]').tab('show');
    })
    $('#ponencia').click(function(e){
      e.preventDefault();
        $('#mytabs a[href="#pon_tab"]').tab('show');
    })

    $('#material').click(function(e){
      e.preventDefault();
        $('#mytabs a[href="#mat_tab"]').tab('show');
    })

    $('#docente').click(function(e){
      e.preventDefault();
        $('#mytabs a[href="#prof_tab"]').tab('show');
    })

  })

    $('form').submit(function(){
    	alert($(this["options"]).val());
        return false;
    });

});