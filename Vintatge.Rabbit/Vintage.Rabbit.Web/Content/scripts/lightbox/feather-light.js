/**
 * Featherlight - ultra slim jQuery lightbox
 * Version 0.4.1 - http://noelboss.github.io/featherlight/
 *
 * Copyright 2014, No�l Raoul Bossart (http://www.noelboss.com)
 * MIT Licensed.
**/
(function ($) {
    "use strict";

    if ('undefined' === typeof $) {
        if ('console' in window) { window.console.info('Too much lightness, Featherlight needs jQuery.'); }
        return;
    }

    /* extend jQuery with standalone featherlight method  $.featherlight(elm, config); */
    function Featherlight($content, config) {
        if (this.constructor === Featherlight) {  /* called with new */
            this.id = Featherlight.id++;
            this.setup($content, config);
        } else {
            var fl = new Featherlight($content, config);
            fl.open();
            return fl;
        }
    }

    /* document wide esc handler, attached in setup method */
    var escapeHelper = function (event) {
        if (27 === event.keyCode && !event.isDefaultPrevented()) { // esc keycode
            var self = Featherlight.current();
            if (self && self.closeOnEsc) {
                self.$instance.find('.' + self.namespace + '-close:first').click();
                event.preventDefault();
            }
        }
    },

		/* read element's attributes starting with data-featherlight- */
		readElementConfigHelper = function (element) {
		    var config = {};
		    if (element && element.attributes) {
		        $.each(element.attributes, function () {
		            var match = this.name.match(/^data-featherlight-(.*)/);
		            if (match) {
		                var val = this.value;
		                try {
		                    val = $.parseJSON(val);
		                }
		                catch (e) { }
		                config[$.camelCase(match[1])] = val;
		            }
		        });
		    }
		    return config;
		};

    Featherlight.prototype = {
        constructor: Featherlight,
        /*** defaults ***/
        /* extend featherlight with defaults and methods */
        namespace: 'featherlight',         /* Name of the events and css class prefix */
        targetAttr: 'data-featherlight',    /* Attribute of the triggered element that contains the selector to the lightbox content */
        variant: null,                   /* Class that will be added to change look of the lightbox */
        resetCss: false,                  /* Reset all css */
        background: null,                   /* Custom DOM for the background, wrapper and the closebutton */
        openTrigger: 'click',                /* Event that triggers the lightbox */
        closeTrigger: 'click',                /* Event that triggers the closing of the lightbox */
        filter: null,                   /* Selector to filter events. Think $(...).on('click', filter, eventHandler) */
        root: 'body',                 /* Where to append featherlights */
        openSpeed: 250,                    /* Duration of opening animation */
        closeSpeed: 250,                    /* Duration of closing animation */
        closeOnClick: 'background',           /* Close lightbox on click ('background', 'anywhere' or false) */
        closeOnEsc: true,                   /* Close lightbox when pressing esc */
        closeIcon: '&#10005;',             /* Close icon */
        otherClose: null,                   /* Selector for alternate close buttons (e.g. "a.close") */
        beforeOpen: $.noop,                 /* Called before open. can return false to prevent opening of lightbox. Gets event as parameter, this contains all data */
        beforeClose: $.noop,                 /* Called before close. can return false to prevent opening of lightbox. Gets event as parameter, this contains all data */
        afterOpen: $.noop,                 /* Called after open. Gets event as parameter, this contains all data */
        afterClose: $.noop,                 /* Called after close. Gets event as parameter, this contains all data */
        type: null,                   /* Specify type of lightbox. If unset, it will check for the targetAttrs value. */
        contentFilters: ['jquery', 'image', 'html', 'ajax', 'text'], /* List of content filters to use to determine the content */

        /*** methods ***/
        /* setup iterates over a single instance of featherlight and prepares the background and binds the events */
        setup: function (target, config) {
            /* all arguments are optional */
            if (typeof target === 'object' && target instanceof $ === false && !config) {
                config = target;
                target = undefined;
            }

            var self = $.extend(this, config, { target: target }),
				css = !self.resetCss ? self.namespace : self.namespace + '-reset', /* by adding -reset to the classname, we reset all the default css */
				$background = $(self.background || '<div class="' + css + '"><div class="' + css + '-content"><span class="' + css + '-close-icon ' + self.namespace + '-close">' + self.closeIcon + '</span></div></div>'),
				closeButtonSelector = '.' + self.namespace + '-close' + (self.otherClose ? ',' + self.otherClose : '');

            self.$instance = $background.clone().addClass(self.variant); /* clone DOM for the background, wrapper and the close button */

            /* attach esc handler to document if configured */
            if (self.closeOnEsc && escapeHelper) {
                $(document).bind('keyup.' + this.constructor.prototype.namespace, escapeHelper);
                escapeHelper = null;
            }

            /* close when click on background/anywhere/null or closebox */
            self.$instance.on(self.closeTrigger + '.' + self.namespace, function (event) {
                var $target = $(event.target);
                if (('background' === self.closeOnClick && $target.is('.' + self.namespace))
                    || 'anywhere' === self.closeOnClick
                    || $target.is(closeButtonSelector)) {
                    event.preventDefault();
                    self.close();
                }
            })
				.on('featherlightGetCurrent', function (event) {
				    if (self.$instance.closest('body').length > 0) {
				        event.currentFeatherlight = self;
				    }
				});

            return this;
        },

        /* this method prepares the content and converts it into a jQuery object or a promise */
        getContent: function () {
            var self = this,
				filters = this.constructor.contentFilters,
				readTargetAttr = function (name) { return self.$currentTarget && self.$currentTarget.attr(name); },
				targetValue = readTargetAttr(self.targetAttr),
				data = self.target || targetValue || '';

            /* Find which filter applies */
            var filter = filters[self.type]; /* check explicit type like {type: 'image'} */

            /* check explicit type like data-featherlight="image" */
            if (!filter && data in filters) {
                filter = filters[data];
                data = self.target && targetValue;
            }
            data = data || readTargetAttr('href') || '';

            /* check explicity type & content like {image: 'photo.jpg'} */
            if (!filter) {
                for (var filterName in filters) {
                    if (self[filterName]) {
                        filter = filters[filterName];
                        data = self[filterName];
                    }
                }
            }

            /* otherwise it's implicit, run checks */
            if (!filter) {
                var target = data;
                data = null;
                $.each(self.contentFilters, function () {
                    filter = filters[this];
                    if (filter.test) {
                        data = filter.test(target);
                    }
                    if (!data && filter.regex && target.match && target.match(filter.regex)) {
                        data = target;
                    }
                    return !data;
                });
                if (!data) {
                    if ('console' in window) { window.console.error('Featherlight: no content filter found ' + (target ? ' for "' + target + '"' : ' (no target specified)')); }
                    return false;
                }
            }
            /* Process it */
            return filter.process.call(self, data);
        },

        /* sets the content of $instance to $content */
        setContent: function ($content) {
            var self = this;
            /* we need a special class for the iframe */
            if ($content.is('iframe') || $('iframe', $content).length > 0) {
                self.$instance.addClass(self.namespace + '-iframe');
            }
            self.$content = $content.addClass(self.namespace + '-inner');

            /* remove existing content */
            self.$instance.find('.' + self.namespace + '-inner').remove();
            self.$instance.find('.' + self.namespace + '-content').append(self.$content);

            return self;
        },

        /* opens the lightbox. "this" contains $instance with the lightbox, and with the config */
        open: function (event) {
            var self = this;
            if (event && !event.isDefaultPrevented()
				|| self.beforeOpen(event) !== false) {

                if (event) {
                    event.preventDefault();
                }
                var $content = self.getContent();

                /* If we have content, add it and show lightbox */
                if ($content) {
                    $.when($content).done(function ($content) {
                        self.setContent($content)
							.$instance.appendTo(self.root).fadeIn(self.openSpeed);
                        self.afterOpen(event);
                    });
                    return self;
                }
            }
            return false;
        },

        /* closes the lightbox. "this" contains $instance with the lightbox, and with the config */
        close: function (event) {
            var self = this;
            if (self.beforeClose(event) === false) {
                return false;
            }
            self.$instance.fadeOut(self.closeSpeed, function () {
                self.$instance.detach();
            });
            $('.lightbox-loading').fadeOut(self.closeSpeed, function () {
                $('.lightbox-loading').remove();
            });
            self.afterClose(event);
        }
    };

    $.extend(Featherlight, {
        id: 0,                                    /* Used to id single featherlight instances */
        autoBind: '[data-featherlight]',    /* Will automatically bind elements matching this selector. Clear or set before onReady */
        defaults: Featherlight.prototype,   /* You can access and override all defaults using $.featherlight.defaults, which is just a synonym for $.featherlight.prototype */
        /* Contains the logic to determine content */
        contentFilters: {
            jquery: {
                regex: /^[#.]\w/,         /* Anything that starts with a class name or identifiers */
                test: function (elem) { return elem instanceof $ && elem; },
                process: function (elem) { return $(elem).clone(true); }
            },
            image: {
                regex: /\.(png|jpg|jpeg|gif|tiff|bmp)(\?\S*)?$/i,
                process: function (url) {
                    var self = this,
						deferred = $.Deferred(),
						img = new Image();
                    img.onload = function () {
                        deferred.resolve(
                            $('<img src="' + url + '" alt="" class="' + self.namespace + '-image" />')
                        );
                    };
                    img.onerror = function () { deferred.reject(); };
                    img.src = url;
                    return deferred.promise();
                }
            },
            html: {
                regex: /^\s*<[\w!][^<]*>/, /* Anything that starts with some kind of valid tag */
                process: function (html) { return $(html); }
            },
            ajax: {
                regex: /./,            /* At this point, any content is assumed to be an URL */
                process: function (url) {
                    var self = this,
						deferred = $.Deferred();
                    if ($('.lightbox-loading').length == 0) {
                        $('body').append('<div class="lightbox-loading">Loading</div>');
                    }
                    /* we are using load so one can specify a target with: url.html #targetelement */
                    var $container = $('<div></div>').load(url, function (response, status) {
                        $('.lightbox-loading').html('');
                        if (status !== "error") {
                            deferred.resolve($container.contents());
                        }
                        deferred.fail();
                    });
                    return deferred.promise();
                }
            },
            text: {
                process: function (text) { return $('<div>', { text: text }); }
            }
        },

        /*** class methods ***/

        attach: function ($source, $content, config) {
            if (typeof $content === 'object' && $content instanceof $ === false && !config) {
                config = $content;
                $content = undefined;
            }
            /* make a copy */
            config = $.extend({}, config);

            /* Only for openTrigger and namespace... */
            var tempConfig = $.extend({}, this.defaults, readElementConfigHelper($source[0]), config);

            $source.on(tempConfig.openTrigger + '.' + tempConfig.namespace, tempConfig.filter, function (event) {
                /* ... since we might as well compute the config on the actual target */
                var elemConfig = $.extend({ $currentTarget: $(this) }, readElementConfigHelper(this), config);
                new $.featherlight($content, elemConfig).open(event);
            });
        },

        current: function () {
            var event = new $.Event('featherlightGetCurrent');
            $.event.trigger(event);
            return event.currentFeatherlight;
        },

        close: function () {
            var cur = this.current();
            if (cur) { cur.close(); }
        }
    });

    $.featherlight = Featherlight;

    /* bind jQuery elements to trigger featherlight */
    $.fn.featherlight = function ($content, config) {
        Featherlight.attach(this, $content, config);
        return this;
    };

    /* bind featherlight on ready if config autoBind is set */
    $(document).ready(function () {
        if (Featherlight.autoBind) {
            $(document).featherlight({ filter: Featherlight.autoBind });
        }
    });
}(jQuery));