// ensure that details elements are displayed open for print

export default function() {
  const $content = $('#main-content');
  let closedDetailElements = [];
  const beforePrint = function() {
    closedDetailElements = [];
    $content.find('details').not('[open]').each(function(n, elem) {
      $(elem).attr('open', true);
      closedDetailElements.push($(elem));
    });
  };
  const afterPrint = function() {
    for (let i = 0, len = closedDetailElements.length; i < len; i++){
      closedDetailElements[i].removeAttr('open');
    }
  };

  if (window.matchMedia) {
    const mediaQueryList = window.matchMedia('print');
    mediaQueryList.addListener(function(mql) {
      if (mql.matches) {
        beforePrint();
      } else {
        afterPrint();
      }
    });
  }

  window.onbeforeprint = beforePrint;
  window.onafterprint = afterPrint;
}
