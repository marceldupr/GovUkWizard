import AppModal from './AppModal';
class AppCancelDialog extends AppModal{
  constructor(el, opts) {
    super(el, opts);

    $(window).on('modal:opened', (e)=> {
      if (e.element === this.el) {
        this.bindButtonEvents();
      }
    });

    $(window).on('modal:closed', ()=> {
      this.unbindButtonEvents();
    });
  }

  bindButtonEvents() {
    $('.app-modal__button-positive').on('click', (e)=>{
      e.preventDefault();
      $('#app-modal-close').click();
      if (this.el.tagName.toLowerCase() === 'a') {
        window.location = this.el.getAttribute('href');
      } else {
        $(this.el).off('click');
        $(this.el).click();
      }

    });

    $('.app-modal__button-negative').on('click',(e)=> {
      e.preventDefault();
      $('#app-modal-close').click();
    });
  }

  unbindButtonEvents() {
    $('.app-modal__button-positive').off('click');
    $('.app-modal__button-negative').off('click');
  }
}


export default AppCancelDialog;
