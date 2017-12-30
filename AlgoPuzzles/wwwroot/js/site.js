﻿(function ($) {
    var showLoader = function ($wrap, show) {
        $wrap.find('.ajax-loader').remove();
        if (show) {
            var $loader = $('<div class="ajax-loader">&nbsp;</div>');
            $loader.prependTo($wrap);
        }
    }

    var responseToList = function (mime, data) {
        var res = '', count = 0;
        if (/application\/json/ig.test(mime)) {

            var obj = typeof data === 'object' ? data : JSON.parse(data);

            if (obj.error && obj.error.message)
                return '<span>' + obj.error.message + '</span>';

            for (var k in obj)
                if (obj.hasOwnProperty(k)) {
                    var item = obj[k];
                    if (Array.isArray(item))
                        for (var i = 0; i < item.length; ++i)
                            res += '<li>' + item[i] + '</li>';
                    else {
                        if (typeof item === 'string')
                            res += '<li>' + item + '</li>';
                        else
                            continue;
                    }
                    ++count;
                }
            if (res === '')
                res = '[empty]';
        } else {
            if (typeof data === 'string' && data.length === 0)
                res = '[empty]';
            else
                res = '<li>' + data + '</li>';
        }

        return count > 1 ? ('<ul style="list-style: disc">' + res + '</ul>') : ('<ul>' + res + '</ul>');
    }

    var hideAlert = function ($item) {
        var $alert = $('.alert', $item).first();
        $alert.slideUp(700, function () {
            //$alert.remove();
        });
    }
    var showAlert = function ($item, $tree) {
        var $alert = $('.alert', $item).first();
        $alert.find('.msg-area').html($tree);
        $alert.slideDown('slow');
    }

    $('#tabs').tabs({
        orientation: "vertical",
        active: $.cookie('currenttab'),
        activate: function (ev, ui) {
            $.cookie('currenttab', ui.newTab.index(), { expires: 30 })
        }
    }).addClass('ui-tabs-vertical ui-helper-clearfix').on('click', 'button[type=submit]', function (ev) {
        var $button = $(ev.target),
            $form = $button.closest('form'),
            $item = $form.closest('.row-item'),
            $resultView = $('.result-area', $item),
            $resultCell = $resultView.parent(),
            action = $form.attr('action');
        ev.stopPropagation();
        event.preventDefault();

        var timerId = 0;
        $.post(
            {
                url: action,
                data: $form.serialize(),
                beforeSend: function () {
                    $button.prop('disabled', true);
                    hideAlert($item);
                    timerId = setTimeout(function () { showLoader($resultCell, true); }, 250)
                }
            })
            .done(function (data, textStatus, jqXHR) {
                var resultView = typeof data === 'object' ? JSON.stringify(data) : data;
                $resultView.html(resultView);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus);
                console.log(errorThrown);
                console.log(jqXHR.responseText);

                showAlert($item, $(responseToList(jqXHR.getResponseHeader("content-type"), jqXHR.responseText)));
            })
            .always(function () {
                clearTimeout(timerId);
                showLoader($resultCell, false);
                $button.prop('disabled', false);                
            });


        return false;
    }).on('click', 'button.close', function (ev) {
        var $item = $(ev.target).closest('.row-item');
        hideAlert($item);
    })
})(jQuery);

