
class Validation {
  constructor() {
    this.init();
  }

  init() {
    this.errors = [];
    this.messageTemplate = '<span class="govuk-error-message">{message}</span>';

    const forms = $('form');
    for (let i = 0; i < forms.length; i++) {
      const form = $(forms[i]);
      const formHasValidationAttributes = form.find(':input').filter((n, input)=> {
        return typeof $(input).data() !== 'undefined' && $(input).data().val === true;
      }).length > 0;

      if (formHasValidationAttributes) {
        form.on('submit', (e)=> {
          this.validateForm(e, form);
        });
      }

    }
  }

  validateDate(dateObj) {
    const day = dateObj.day;
    const month = dateObj.month;
    const year = dateObj.year;

    let dateError = false;
    const months31 = [0, 2, 4, 6, 7, 9, 11];

    if (isNaN(day) || isNaN(month) || isNaN(year)) {
      dateError = true;
    }

    const isLeap = new Date(year, 1, 29).getMonth() === 1;

    if (isLeap && month === 1) {
      if (day > 29) {
        dateError = true;
      }
    } else if (month === 1) {
      if (day > 28) {
        dateError = true;
      }
    }

    if (months31.indexOf(month - 1)) {
      if (day < 1 || day > 31) {
        dateError = true;
      }
    } else {
      if (day < 1 || day > 30) {
        dateError = true;
      }
    }

    if (month < 0 || month > 11) {
      dateError = true;
    }

    return dateError;
  }

  clearErrors(form) {
    $(form).find('.govuk-form-group').removeClass('govuk-form-group--error');
    $(form).find(':input').removeClass('govuk-input--error');
    $(form).find('.govuk-error-message').remove();
    this.errors = [];
  }

  showErrorSummary() {
    let errorHtml = [];
    const summaryContainer = $('#error-summary-container');

    summaryContainer.find('.govuk-error-summary__list').empty();

    if (this.errors.length > 0) {
      for (let i = 0; i < this.errors.length; i++) {
        errorHtml.push(`<li><a href="#${this.errors[i].ref}">${this.errors[i].message}</a></li>`);
      }
      summaryContainer.find('.govuk-error-summary__list').append(errorHtml.join(''));
      summaryContainer.removeClass('hidden').focus();
      window.scrollTo(0,0);
    }
  }

  validateForm(e, form) {
    this.clearErrors(form);

    e.preventDefault();

    const fields = $(form).find(':input').filter((n, input) =>{
      return typeof $(input).data() !== 'undefined' && $(input).data().val === true;
    });

    for (let i = 0; i < fields.length; i++) {
      const field = $(fields[i]);
      const fieldType = field.prop('tagName').toLowerCase() === 'textarea' ? 'textarea' : $(field).attr('type');
      let message;

      if ((fieldType === 'text' || fieldType === 'textarea') && field.val() === '') {
        message = this.messageTemplate.replace('{message}', $(field).data().valRequired);

        field.before(message);
        field.addClass('govuk-input--error');
        field.parent('.govuk-form-group').addClass('govuk-form-group--error');

        this.errors.push({
          ref: field.attr('id'),
          message: $(field).data().valRequired
        });

      }

      if (fieldType === 'radio' || fieldType === 'checkbox') {
        const radioName = $(field).prop('name');
        const isChecked = $('[name="'+radioName+'"]').filter(':checked').length > 0;
        if(!isChecked) {
          message = this.messageTemplate.replace('{message}', $(field).data().valRequired);

          $(field).parents('.govuk-form-group').addClass('govuk-form-group--error');
          $(field).parents('.govuk-form-group').find('legend').after(message);

          this.errors.push({
            ref: field.attr('id'),
            message: $(field).data().valRequired
          });
        }
      }



      if (i === fields.length -1) {
        if (this.errors.length === 0) {
          form.off('submit');
          form.submit();
        } else {
          this.showErrorSummary();

        }
      }
    }
  }
}

export default Validation;
