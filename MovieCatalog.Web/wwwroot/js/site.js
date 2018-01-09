// Global constants
var cast = 'Cast';
var crew = 'Crew';
var noOptionsMessage = '* No options to move!';
var selectOptionMessage = '* You must select an option!';

// Extend :contains selector for case insensitive search & regex
// Use it as containsCI
$.extend(
    jQuery.expr[':'].containsCI = function (a, i, m) {
        //-- faster than jQuery(a).text()
        let sText = a.textContent || a.innerText || "";
        let zRegExp = new RegExp(m[3], 'i');
        return zRegExp.test(sText);
    }
);

// Sort options in select list
$.fn.sortOptions = function () {
    $(this).each(function () {
        let op = $(this).children("option");
        op.sort(function (a, b) {
            return a.text.localeCompare(b.text);
        });
        return $(this).empty().append(op);
    });
};

// Set main content height
$(function () {
    //setHeight();
    // Set main content height on resize
    $(window).resize(function () {
        setHeight();
    });
    function setHeight() {
        let header = 0;
        let navbar = 0;
        let footer = 0;
        if ($('header').length) {
            header = $('header').outerHeight(true);
        }
        if ($('.navbar').length) {
            navbar = $('.navbar').outerHeight(true);
        }
        if ($('footer').length) {
            footer = $('footer').outerHeight(true);
        }
        let elems = header + navbar + footer;
        let height = $(window).outerHeight(true) - elems;
        //console.log(height);
        //console.log(navbar);
        $('main').height(height);
        $('main').css('padding-top', navbar);
    }
});

// Set countdown timer for closing on main alert
$(function () {
    let selector = '#my-main-alert';
    let timer, counter = $(`${selector} span`).text();
    $(selector).delay(counter * 1000)
        .slideToggle(1000, function () { $(this).remove(); });
    timer = setInterval(function () {
        $(`${selector} span`).html(--counter);
        if (counter === 0) { clearInterval(timer); }
    }, 1000);
});

// Show tooltip for truncated text
$(document).on('mouseenter', '.text-truncate', function () {
    let $this = $(this);
    if (this.offsetWidth < this.scrollWidth && !$this.attr('title')) {
        $this.tooltip({
            title: $this.text(),
            placement: "top"
        });
        $this.tooltip('show');
    }
});

// Move all options from one select list to another
function moveAllOptions(selectorFrom, selectorTo, isRemoveOptsFrom) {
    let allOpts = $(`${selectorFrom} option`);

    if (allOpts.length === 0) {
        showAlert(noOptionsMessage);
    }
    else {
        $(selectorTo)
            .append($(allOpts).clone())
            .sortOptions();
        if (isRemoveOptsFrom) {
            $(allOpts).remove();
        }
    }
}

// Move one option from one select list to another
function moveOneOption(selectorFrom, selectorTo, isRemoveOptsFrom) {
    let allOpts = $(`${selectorFrom} option`);
    let selectedOpts = $(`${selectorFrom} option:selected`);

    if (allOpts.length === 0) {
        showAlert(noOptionsMessage);
    }
    else if (selectedOpts.length === 0) {
        showAlert(selectOptionMessage);
    }
    else {
        $(selectorTo)
            .append($(selectedOpts).clone())
            .sortOptions();
        if (isRemoveOptsFrom) {
            $(selectedOpts).remove();
        }
    }
}

// Show alert on select list
function showAlert(alertMessage) {
    let elem = $('.note-list');
    let note = elem.text();
    elem.addClass('text-danger').text(alertMessage);
    setTimeout(function () { elem.removeClass('text-danger').text(note); }, 1000);
}

// Search box in index left panel
$(function () {
    $('#main-search').keyup(function () {
        // Search text
        let text = $(this).val().toLowerCase();
        // Hide all content class element
        $('.card').hide();
        // Search and show
        $(`.card:containsCI(${text})`).show();
    });
});

// Thumbnails & Back buttons
$(document).on('click', '.ajax-thumbnail', function (e) {
    e.preventDefault();
    e.stopPropagation();
    // Load Details href if thumbnail image is clicked
    if ($(this).hasClass('ajax-thumbnail')) {
        $('#col-panel-right').load(this.href);
    }
    // Load Details href if Back button is clicked
    else {
        history.back();
    }

    // Change url on partial view load & log history
    if (history.pushState) {
        history.pushState({}, '', $(this).attr('href'));
    }
});

// Add cast row in create form
$(document).on('click', '#btn-add-cast', function (e) {
    e.preventDefault();
    e.stopPropagation();
    let selector = $('#row-cast');
    addFieldset(selector);
    countFieldsets('increment', cast);
});

// Add crew row in create form
$(document).on('click', '#btn-add-crew', function (e) {
    e.preventDefault();
    e.stopPropagation();
    let selector = $('#row-crew');
    addFieldset(selector);
    countFieldsets('increment', crew);
});

// Remove cast fieldset button
$(document).on('click', '.btn-remove-cast', function (e) {
    e.preventDefault();
    e.stopPropagation();
    let fieldset = $(this).closest('fieldset');
    removeFieldset(fieldset, cast);
});

// Remove crew fieldset button
$(document).on('click', '.btn-remove-crew', function (e) {
    e.preventDefault();
    e.stopPropagation();
    let fieldset = $(this).closest('fieldset');
    removeFieldset(fieldset, crew);
});

function addFieldset(selector) {
    let fieldsets = selector.children('fieldset');
    if (fieldsets.length === 1 && fieldsets.first().is(':disabled')) {
        fieldsets.first().prop('disabled', false);
    }
    else {
        let clone = fieldsets.last().clone();
        clone.appendTo(selector).find('input').val('');
        setAttrIndex(selector);
    }
}

function removeFieldset(selector, value) {
    let parent = selector.parent();
    if (parent.children().length > 1) {
        selector.remove();
        countFieldsets('decrement', value);
        setAttrIndex(parent);
    }
    else {
        selector.prop('disabled', function (i, v) { return !v; });
        if (selector.is(':disabled')) {
            countFieldsets('decrement', value);
        }
        else {
            countFieldsets('increment', value);
        }
        // Change dropdown list to Select
        if (selector.is(':disabled') && value === crew) { $('#crew-role').val(''); }
    }
}

function countFieldsets(op, value) {
    let span = $(`h4:contains(${value}) span`);
    let count = Number(span.text());
    if (op === 'increment') {
        span.text(++count);
    }
    else if (op === 'decrement') {
        span.text(--count);
    }
}

// Updates the name value [index] (important for child collection model binding)
function setAttrIndex(selector) {
    selector.children('fieldset').each(function (i, v) {
        $(this).find('input').each(function () {
            let nameValue = $(this).attr('name');
            $(this).attr('name', nameValue.replace(/\[\d+\]/, `[${i}]`));
        });
        $(this).find('select').each(function () {
            let idValue = $(this).attr('id');
            let nameValue = $(this).attr('name');
            $(this).attr('id', idValue.replace(/\[\d+\]/, `[${i}]`));
            $(this).attr('name', nameValue.replace(/\[\d+\]/, `[${i}]`));
        });
        $(this).find('span').each(function () {
            let valValue = $(this).attr('data-valmsg-for');
            $(this).attr('data-valmsg-for', valValue.replace(/\[\d+\]/, `[${i}]`));
        });
    });
}

// Delete button
$(document).on('click', '.btn-delete-movie', function (e) {
    e.preventDefault();
    e.stopPropagation();
    let href = this.href;
    let id = $(this).attr('value');
    let title = $(this).attr("data-value");
    let data = { id: id, title: title };
    swal({
        title: 'Choose wisely',
        html: `<em>${title}</em> will be irreversibly deleted.`,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
        showLoaderOnConfirm: true,
        preConfirm: function () {
            return new Promise(function (resolve) {
                $.ajax({
                    url: href,
                    type: 'GET',
                    data: data,
                    dataType: 'json'
                })  // Success callback
                    .done(function (response, status, jqxhr) {
                        swal({
                            title: 'Success',
                            html: response.message,
                            type: 'success',
                            onClose: redirect
                        });
                        function redirect() {
                            window.location.href = '/';
                        }
                    })  // Error callback
                    .fail(function (jqxhr, status) {
                        let errorMessage = `The server returned status code ${jqxhr.status}.`;
                        swal({
                            title: 'Oops...',
                            text: errorMessage,
                            type: 'error'
                        });
                    });
            });
        },
        allowOutsideClick: false
    });
});

// Search button (Add movie automatically)
$(document).on('click', "#search-movie", function (e) {
    e.preventDefault();
    e.stopPropagation;
    let href = this.href;
    swal({
        title: '<h4>Input title to search online</h4>',
        input: 'text',
        showCancelButton: true,
        confirmButtonText: 'Submit',
        inputValidator: (value) => {
            return new Promise((resolve) => {
                if (value) {
                    resolve();
                } else {
                    resolve('You need to write something!');
                }
            });
        }
    }).then((result) => {
        if (result.value) {
            loadSpinningGif();
            $.ajax({
                method: 'GET',
                url: href,
                data: { title: result.value },
                dataType: 'html'
            })  // Success callback
                .done(function (response, status, jqxhr) {
                    $('#main-container').html(response);
                })  // Error callback
                .fail(function (jqxhr, status) {
                    let errorMessage = `The server returned status code ${jqxhr.status}.`;
                    swal({
                        title: 'Oops...',
                        text: errorMessage,
                        type: 'error'
                    });
                });
        }
    });
});

// Submit search button
//$(document).on('click', '#btn-submit-search', function (e) {
//    e.preventDefault();
//    e.stopPropagation();
//    loadSpinningGif();
//});

//$('#btn-submit-search').mouseup(function () {
//    loadSpinningGif();
//});

// Load spinning gif
function loadSpinningGif() {
    //let gif = '<div class="mt-5 text-center" id="loading-gif"><h2>Collecting data. Please wait... <span id="minutes"></span>:<span id="seconds"></span></h2><i class="fa fa-circle-o-notch fa-spin" style="font-size: 12.5rem"></i></div>';
    let gif = '<div class="mt-5 text-center" id="loading-gif"><h2>Collecting data. Please wait...</h2><i class="fa fa-circle-o-notch fa-spin" style="font-size: 12.5rem"></i></div>';
    $('#main-container').html(gif);
}

// Set countup timer for waiting while searching
$(function () {
    let sec = 0;
    function pad(val) { return val > 9 ? val : "0" + val; }
    setInterval(function () {
        $("#seconds").html(pad(++sec % 60));
        $("#minutes").html(pad(parseInt(sec / 60, 10)));
    }, 1000);
});

// Force open artsst & search links in external window
$(document).on('click', '.external-link', function (e) {
    e.preventDefault();
    let url = $(this).attr('href');
    window.open(url, '_blank');
});
