/**
 * Modules that are instantiated on every page
 */

import {initAll} from 'govuk-frontend';
import accordionExtensions from '../AppModules/GovUkComponentExtensions/appAccordionExtensions';
import AppRibbonNavigation from '../AppModules/Navigation/AppRibbonNavigation';
import AppCancelDialog from '../AppModules/AppModals/AppCancelDialog';
import Validation from '../AppModules/validation';

initAll();
accordionExtensions();

if (document.getElementById('app-ribbon-nav')) {
  const ribbonNav = new AppRibbonNavigation();
  let resizeTimer;

  $(window).on('resize', function() {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(function() {
      ribbonNav.reinit();

    }, 750);
  });
}


$('.app-modal__cancel-link').each(function(n, el) {
  new AppCancelDialog(el, {
    contentSelector: '#confirm-cancel-amendment',
    hideTitle: true,
    additionalClasses: 'app-modal__yes-no'
  });
});

new Validation();
