/** 
  * Template Name: Varsity
  * Version: 1.0  
  * Template Scripts
  * Author: MarkUps
  * Author URI: http://www.markups.io/

  Custom JS
  

  1. SEARCH FORM
  2. ABOUT US VIDEO
  2. TOP SLIDER
  3. ABOUT US (SLICK SLIDER) 
  4. LATEST COURSE SLIDER (SLICK SLIDER) 
  5. TESTIMONIAL SLIDER (SLICK SLIDER)
  6. COUNTER
  7. RELATED ITEM SLIDER (SLICK SLIDER)
  8. MIXIT FILTER (FOR GALLERY)
  9. FANCYBOX (FOR PORTFOLIO POPUP VIEW)  
  11. HOVER DROPDOWN MENU
  12. SCROLL TOP BUTTON  

  
**/

jQuery(function($){

  /* ----------------------------------------------------------- */
  /*  11. HOVER DROPDOWN MENU
  /* ----------------------------------------------------------- */ 
  
  // for hover dropdown menu
    jQuery('ul.nav li.dropdown').hover(function() {
      jQuery(this).find('.dropdown-menu').stop(true, true).delay(200).fadeIn(200);
    }, function() {
      jQuery(this).find('.dropdown-menu').stop(true, true).delay(200).fadeOut(200);
    });

    
  /* ----------------------------------------------------------- */
  /*  12. SCROLL TOP BUTTON
  /* ----------------------------------------------------------- */

  //Check to see if the window is top if not then display button

    jQuery(window).scroll(function(){
      if (jQuery(this).scrollTop() > 300) {
        jQuery('.scrollToTop').fadeIn();
      } else {
        jQuery('.scrollToTop').fadeOut();
      }
    });
     
    //Click event to scroll to top

    jQuery('.scrollToTop').click(function(){
      jQuery('html, body').animate({scrollTop : 0},800);
      return false;
    });  
});



function showMessage(obj, messageObj, href) {
    if (messageObj != "") {

        // Convert message from the server into json and get div element
        var message = JSON.parse(messageObj.replace(/&quot;/g, '"'));
        var element = document.getElementById(obj);

        // Modify div name with message data
        element.classList.add("alert-" + message.Type);
        var s = document.createElement("strong");
        s.appendChild(document.createTextNode(message.Title));
        element.appendChild(s);
        element.appendChild(document.createTextNode(" " + message.Message));
        element.hidden = false;

        // If success redirect to mentees options
        if (message.Type == "success") {
            setTimeout(() => { window.location.href = href; }, 1500);
        };
    };
};