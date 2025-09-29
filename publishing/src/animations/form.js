import jQuery from 'jquery';
var $ = window.jQuery || jQuery;

function createSelect($select) {
    var placeholder = '<span class="placeholder">' + $select.attr('placeholder') + '</span>';
    var $customSelect = $('<div class="custom-select"><div class="input ' + $select.attr('class') + '"></div><ul></ul></div>');
    $select.find('option').each(function (idx, el) {
        var $option = $('<li>' + el.innerText + '</li>');
        if (el.value == '') {
            $option.addClass('empty');
        }
        $option.on('click', function () {
            $select.val(el.value).trigger('change');
            $customSelect.removeClass('selected');
        });
        $customSelect.find('ul').append($option);
    });

    var onfocus = function () {
        if ($customSelect.hasClass('selected')) {
            $customSelect.removeClass('selected');
        } else {
            $customSelect.addClass('selected');
        }
    };

    $customSelect.find('.input').on('click', onfocus);

    document.addEventListener('click', function (event) {
        if (!$(event.target).closest($customSelect).length) {
            $customSelect.removeClass('selected');
        }
    });

    $select.on('change', function () {
        if ($(this).val()) {
            $select.addClass('selected');
            $customSelect.find('.input').html($(this).find('option:selected').text());
        } else {
            $select.removeClass('selected');
            $customSelect.find('.input').html(placeholder);
        }

        this.dispatchEvent(new CustomEvent('changed', { bubbles: true, detail: $(this).val() }));
    });

    if ($select.find('option:selected').val()) {
        $customSelect.find('.input').html($select.find('option:selected').text());
    } else {
        $customSelect.find('.input').html(placeholder);
    }

    $select.attr('style', 'display: none !important;');
    $select.parent().append($customSelect);
}

export function FormInit() {
    $('.form-field select').each(function () {
        createSelect($(this));
    });
}