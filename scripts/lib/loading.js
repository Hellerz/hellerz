(function ($) {
    var defaults = {
        debug: false,
        length: 20,
        radius: 6,
        color: 'black',
    };
    // Constructor

    var Loading = function ($e, options) {
        var _namespaces = 'jloading',
           _data = $e.data(_namespaces),
           _userOptions = options,
           _options = $.extend({}, defaults, _userOptions),
           _$element = $e;
        $e.data('ns', _namespaces);
        $e.data(_namespaces, $.extend({}, _data, { initialized: true, waiting: false }, _options));

        _load();
        function _load() {
            var _spinner = $('<div class="spinner"></div>').css({
                'width': _Data().length + 'px',
                'height': _Data().length + 'px'
            }),
                i;
            for (i = 1; i < 4; i++) {
                _spinner.append(_generate_spinner_container(i));
            }
            _$element.append(_spinner);
        };
        function _generate_spinner_container(index) {
            var container = $('<div class="spinner-container container' + index + '"></div>'),
                i;
            for (i = 1; i < 5; i++) {
                container.append($('<div class="circle' + i + '"></div>').css({
                    'background-color': _Data().color,
                    'width': _Data().radius + 'px',
                    'height': _Data().radius + 'px'
                }));
            }
            return container;
        };
        function _Data(data) {
            var tmpOption = _$element.data(_namespaces);
            if (data) {
                tmpOption = $.extend(true, tmpOption, data);
                _$element.data(_namespaces, tmpOption);
            }
            return tmpOption;
        };
        function _destroy() {
            return _$element.removeData(_$element.data('ns')).empty();
        }
        $.extend(_$element.jloading, {
            destroy: _destroy
        });
        return _$element;
    };

    $.fn.loading = function (option) {
        return this.each(function () {
            var $this = $(this),
                data = $this.data('jloading');
            if (data && data.initialized) return;
            var loading = new Loading($this, option);
        });
    };

})(jQuery);