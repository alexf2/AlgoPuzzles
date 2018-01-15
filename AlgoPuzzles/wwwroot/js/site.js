(function ($) {
    var showLoader = function ($wrap, show) {
        $wrap.find('.ajax-loader').remove();
        if (show) {
            var $loader = $('<div class="ajax-loader">&nbsp;</div>');
            $loader.prependTo($wrap);
        }
    }

    var responseToList = function (mime, jqXHR) {
        var res = '', count = 0, data = jqXHR.responseText;
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
                res = '<span>' + jqXHR.status + ': ' + jqXHR.statusText + '</span>';
            else
                res = '<li>' + data + '</li>';
        }

        return count > 1 ? ('<ul style="list-style: disc">' + res + '</ul>') : ('<ul>' + res + '</ul>');
    }

    var hideAlert = function ($item) {
            var $alert = $('.alert', $item).first();
            $alert.slideUp(700, function () {
        });
    }
    var showAlert = function ($item, $tree) {
        var $alert = $('.alert', $item).first();
        $alert.find('.msg-area').html($tree);
        $alert.slideDown('slow');
    }    
    
    var showCode = function ($container, codeAsString) {        
        $container.find('#code-panel').remove();
        var $template = $('<div class="panel panel-primary" id="code-panel" role="alert">\
            <div class="panel-heading">Source Code\
                <button type= "button" class="close close-code" data-target="#code-panel" data-dismiss="alert" aria-label="Close"><span aria-hidden="true"><big>&times;</big></span>\
                        </button>\
                    </div>\
        <div class="panel-body"><pre><code class="lang-csharp panel-inner-body">&nbsp;</code></pre></div>\
                </div>');
        $template.find('.panel-inner-body').text(codeAsString).each(function (i, block) {
            hljs.highlightBlock(block);
        });
        $container.prepend($template);
    }

    $('#tabs').tabs({
        orientation: "vertical",
        active: $.cookie('currenttab'),
        activate: function (ev, ui) {
            $.cookie('currenttab', ui.newTab.index(), { expires: 30 })
        }
    })
    .show()
    .addClass('ui-tabs-vertical ui-helper-clearfix').on('click', 'button[type=submit]', function (ev) {
        var $button = $(ev.target),
            $form = $button.closest('form'),
            $item = $form.closest('.row-item'),
            $resultView = $('.result-area', $item),
            $resultCell = $resultView.parent(),
            action = $form.attr('action');

        ev.stopPropagation();
        ev.preventDefault();

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

                showAlert($item, $(responseToList(jqXHR.getResponseHeader("content-type"), jqXHR)));
            })
            .always(function () {
                clearTimeout(timerId);
                showLoader($resultCell, false);
                $button.prop('disabled', false);
            });


        return false;
    })    
    .on('click', '#btn-code', function (ev) {                        
        var $button = $(ev.target),
            $rootTabContainer = $button.closest('*[data-algo-id]'),
            action = BASE_URL + '/home/code/' + $rootTabContainer.data('algo-id');

        ev.stopPropagation();
        ev.preventDefault();
                
        $.get({
                url: action,                
                beforeSend: function () {
                    $button.prop('disabled', true);
                    hideAlert($rootTabContainer);
                }
            })
            .done(function (data, textStatus, jqXHR) {
                showCode($rootTabContainer, data);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus);
                console.log(errorThrown);
                console.log(jqXHR.responseText);

                $button.prop('disabled', false);
                showAlert($rootTabContainer, $(responseToList(jqXHR.getResponseHeader("content-type"), jqXHR)));
            })
            .always(function () {                
                //$button.prop('disabled', false);
            });
    })
    .on('click', '.alert > button.close', function (ev) {
        var $item = $(ev.target).closest('.row');
        hideAlert($item);
    })
    .on('click', '#code-panel button.close', function (ev) {
        $(ev.target).closest('.panel-group').find('#btn-code').prop('disabled', false);        
    });
})(jQuery);

