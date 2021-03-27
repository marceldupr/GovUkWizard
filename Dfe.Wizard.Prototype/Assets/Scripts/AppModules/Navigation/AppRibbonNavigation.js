class AppRibbonNavigation {
  constructor() {
    this.container = $('.app-ribbon-nav__list');
    this.links = $('.app-ribbon-nav__list-item');

    if ($(window).width() <= 767 && this.container.length > 0) {
      this.init();
    }
  }

  init() {
    let rowY = this.links.eq(0).offset().top;
    let overspillCount = 0;
    let tabBarWidth = this.container.width() -100;
    const self = this;

    $.each(this.links, function(n, lnk) {
      let offset = $(lnk).offset();
      let topShift = $(lnk).height();

      if (offset.top > rowY || offset.left + $(lnk).outerWidth() > tabBarWidth) {
        const hiddenLinks = $(self.links).slice(n);
        hiddenLinks.addClass('app-ribbon-nav__overflow-item hidden');

        $(lnk).before(`<li class="app-ribbon-nav__list-item--more">
                        <a href="#" role="button" id="nav-expander" class="app-ribbon-nav__link govuk-link--no-visited-state">
                          <span class="open-close-text">More</span>
                          <span class="open-close-text hidden">Less</span>
                        </a>
                      </li>`);

        $.each(hiddenLinks, function(n, lnk) {
          overspillCount++;
          $(lnk).css({top: topShift * overspillCount + 'px'});
        });

        return false;
      }
    });

    $('#nav-expander').on('click', function(e) {
      e.preventDefault();
      $(this).find('.open-close-text').toggleClass('hidden');
      $(this).parent().toggleClass('open-extras');
      $('.app-ribbon-nav__overflow-item').toggleClass('hidden');
    });
  }

  reinit() {
    $('#nav-expander').off('click');
    this.container.find('.app-ribbon-nav__list-item--more').remove();
    this.links.removeClass('app-ribbon-nav__overflow-item hidden');

    if ($(window).width() <= 767 && this.container.length > 0) {
      this.init();
    }
  }
}

export default AppRibbonNavigation;
