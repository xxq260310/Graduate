$(function () {
    $(".datefield").datepicker({
        dateFormat: 'm/d/y', changeYear: true, yearRange: '-10:+20', closeAtTop: true,
        closeText:'Clear',
        beforeShow: function( input ) {
            setTimeout(function() {
                var clearButton = $(input )
                    .datepicker( "widget" )
                    .find( ".ui-datepicker-close" );
                clearButton.unbind("click").bind("click",function(){$.datepicker._clearDate( input );});
            }, 1 );}
    })});