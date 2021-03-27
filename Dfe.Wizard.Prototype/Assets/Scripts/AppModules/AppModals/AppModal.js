const defaults = {
  modalTitle: '',
  hideTitle: false,
  contentSelector: '',
  additionalClasses: '',
};

class AppModal {
  constructor(el, opts) {
    this.el = el;
    const metaData = $(this.el).data();
    this.opts = $.extend({}, defaults, metaData, opts);

    this.init();
  }

  init() {

    if (!document.getElementById('app-container')) {
      const $body = $('body');
      $body.wrapInner('<div id="app-container"></div>');
    }

    $(this.el).on('click', (e) => {
      e.preventDefault();
      this.openModal();
    });
  }

  openModal() {
    const opts = this.opts;
    const modalContent = $(opts.contentSelector).html();
    const $appContainer = $('#app-container');
    const titleClasses = opts.hideTitle ? 'app-modal__title hidden' : 'app-modal__title';
    const ariaLabeledBy = opts.hideTitle ? 'app-modal-content-wrapper' : 'app-modal-title';

    const modal = `
        <dialog id="app-modal" open class="app-modal ${opts.additionalClasses}" aria-labelledby="${ariaLabeledBy}" role="dialog">
            <div role="document">
                <div class="app-modal__close-container">
                    <a href="#" role="button" id="app-modal-close" class="app-modal__close-button">Close</a>
                </div>
                <h1 id="app-modal-title" class="${titleClasses}">${opts.modalTitle}</h1>
                <div class="app-modal__content-wrapper" id="app-modal-content-wrapper">
                    <div class="app-modal__content" id="app-modal-content">
                        ${modalContent.toString()}
                      </div>
                </div>
            </div>
        </dialog>`;

    const modalOverlay = `
      <div class="app-modal__overlay" id="app-modal-overlay" title="Close modal">
        <div class="govuk-visually-hidden">Dismiss modal and return to page</div>
      </div>`;

    //this.openedBy = e.target || document.body;

    $('html').addClass('no-scroll');
    $appContainer.attr('aria-hidden', true);

    $(modalOverlay).insertAfter($appContainer);
    $(modal).insertAfter($appContainer);

    $('#app-modal-close').focus();

    $('body').on('keydown', '#app-modal',(e)=> {
      this.manageModalKeyPress(e);
    });

    $('#app-modal-close, #app-modal-overlay').on('click', (e)=> {
      e.preventDefault();
      this.closeModal();
    });

    document.getElementById('app-modal-content-wrapper').addEventListener('touchstart', function(){
      const $appModal = $('#app-modal');
      const top = $appModal.scrollTop;
      const totalScroll = $appModal.scrollHeight;
      const currentScroll = top + $appModal.offsetHeight ;
      if(top === 0) {
        $('#js-modal').scrollTop = 1;
      } else if(currentScroll === totalScroll) {
        $('#js-modal').scrollTop = top - 1;
      }
    });

    $(window).trigger({
      type: 'modal:opened',
      element: this.el
    });
  }

  closeModal() {
    const $body = $('body');
    const $appContainer = $('#app-container');
    $('html').removeClass('no-scroll');
    $appContainer.removeAttr('aria-hidden');

    $body.off('keydown', '#app-modal');

    $('#app-modal-overlay').off('click').remove();
    $('#app-modal').remove();

    this.el.focus();
    $(window).trigger('modal:closed');
  }

  manageModalKeyPress(e) {
    if (e.keyCode === 27) { // esc
      this.closeModal();
      return false;
    }
    if (e.keyCode === 9) { // tab or maj+tab
      const focusableItems = $('#app-modal').find('a[href], area[href], input:not([disabled]), button:not([disabled])').filter(':visible');
      const focusableItemsCount = focusableItems.length;

      const focusedItem = $(document.activeElement);

      const focusedItemIndex = focusableItems.index(focusedItem);

      if (!e.shiftKey && (focusedItemIndex === focusableItemsCount - 1)) {
        focusableItems.get(0).focus();
        e.preventDefault();
      }
      if (e.shiftKey && focusedItemIndex === 0) {
        focusableItems.get(focusableItemsCount - 1).focus();
        e.preventDefault();
      }
    }
  }
}


export default AppModal;

