(function ($) {

    function PhotoRemoval(images, options) {
        return this.initialize(images, options);
    }

    $.fn.photoRemoval = function (options) {

        return new PhotoRemoval(this, options);
    };

    PhotoRemoval.prototype = {

        _selector: null,

        initialize: function (images, options) {
            var $this = this;

            this.addClickEvents(images);

            $(window).on('PhotoRotated', function (event, imageContainer) { $this.addClickEvents(imageContainer); });
            $(window).on('PhotoAdded', function (event, imageContainer) { $this.addClickEvents(imageContainer); });
        },

        addClickEvents: function (images) {
            var $this = this;

            $.each($(images).find('a.remove-photo'), function (index, element) {

                $(element).off('click');
                $(element).on('click', function (event) { event.preventDefault();
                    $this.remove($(this));
                });
            });

        },

        remove: function (element) {
            var $this = this;

            $(element).parents('li').fadeTo('600', 0, function () {

                $(element).parents('li').remove();

                $(window).trigger('PhotoRemoved');

                $.ajax({
                    url: $(element).attr('href'),
                    type: 'POST',
                    success: function (response) { }
                });
            });
        }
    };

    function PhotoSorter(sortContinaer, options) {
        return this.initialize(sortContinaer, options);
    }

    $.fn.photoSorter = function (options) {

        return new PhotoSorter(this, options);
    };

    PhotoSorter.prototype = {

        _sortUrl: null,
        _sortContinaer: null,

        initialize: function (sortContinaer, options) {
            var $this = this;

            this._sortUrl = options.sortUrl;
            this._sortContinaer = sortContinaer;

            this.addSortEvents();

            $(window).on('PhotoRotated', function (event, imageContainer) { $this.addSortEvents(); });
            $(window).on('PhotoAdded', function (event, imageContainer) { $this.addSortEvents(); });
        },

        addSortEvents: function () {
            var $this = this;

            this._sortContinaer.unbind('sort');
            this._sortContinaer.sortable({
                delay: 10,
                items: 'li',
                opacity: 0.6,
                placeholder: 'placeholder',
                revert: 200,
                tolerance: 'pointer',
                cursor: 'pointer',
                update: function () { $this.saveSortOrder(); }
            });

        },

        saveSortOrder: function (element) {
            var $this = this;

            var $this = this;
            var newOrder = [];

            this._sortContinaer.find('li').each(function () {
                newOrder.push($(this).attr('data-id'));
            });

            $(window).trigger('PhotosSorted');

            $.ajax({
                type: 'POST',
                url: this._sortUrl,
                traditional: true,
                data: { photoIds: newOrder },
                cache: false,
                success: function (response) { }
            });
        }
    };

    function PhotoRotator(images, options) {
        return this.initialize(images, options);
    }

    $.fn.photoRotator = function (options) {

        return new PhotoRotator(this, options);
    };

    PhotoRotator.prototype = {

        initialize: function (images, options) {
            var $this = this;

            this.addClickEvents(images);

            $(window).on('PhotoAdded', function (event, imageContainer) { $this.addClickEvents(imageContainer); });
            $(window).on('PhotoRotated', function (event, imageContainer) { $this.addClickEvents(imageContainer); });
        },

        addClickEvents: function (images) {
            var $this = this;

            $.each($(images).find('a.rotate-photo'), function (index, element) {

                $(element).off('click');
                $(element).on('click', function (event) { event.preventDefault();
                    $this.rotate($(this));
                });
            });

        },

        rotate: function (element) {
            var parent = $(element).parents('li');
            parent.append($('<div class="ajax-wait" />'));

            $.ajax({
                url: $(element).attr('href'),
                type: 'POST',
                success: function (response) {

                    if (response != '') {

                        $(response).find('img').load(function () {
                            var id = $(response).data('id');
                            parent.replaceWith(response);
                            $(window).trigger('PhotoRotated', $('li[data-id="' + id + '"]'));
                        });
                    }
                    else {
                        $(element).parents('li').fadeTo('500', 1);
                    }
                }
            });
        }
    };

    function PhotoCounterUpdater(container, options) {
        return this.initialize(container, options);
    }

    $.fn.photoCounterUpdater = function (options) {

        return new PhotoCounterUpdater(this, options);
    };

    PhotoCounterUpdater.prototype = {

        _maxPhotos: null,
        _primaryPhotos: null,

        initialize: function (container, options) {
            var $this = this;

            this._maxPhotos = options.maxPhotos;
            this._primaryPhotos = options.primaryPhotos;

            this.refreshCount();

            $(window).on('PhotoAdded', function (event, imageContainer) { $this.refreshCount(); });
            $(window).on('PhotoRemoved', function (event, imageContainer) { $this.refreshCount(); });
            $(window).on('PhotosSorted', function (event, imageContainer) { $this.refreshCount(); });
            $(window).on('PhotoRotated', function (event, imageContainer) { $this.refreshCount(); });
            $(window).bind('ProductAdded ShoppingCartItemRemoved', function (event, shoppingCart) { $this.shoppingCartChanged(shoppingCart); });
        },

        refreshCount: function (images) {

            var imageCount = $('.photo-list li').length;

            $('.photo-count').html(imageCount + ' of ' + this._maxPhotos + ' photos');

            if (imageCount >= this._maxPhotos) {
                $('.photos').addClass('full');
            }
            else {
                $('.photos').removeClass('full');
            }

            if (imageCount == 0) {
                $('.photos').addClass('empty');
            }
            else {
                $('.photos').removeClass('empty');
            }

            if ($('.photo-list li').length > this._maxPhotos) {
                $('.photo-list li:gt(' + (this._maxPhotos - 1) + ')').remove();
            }

            $('.photo-list li').removeClass('primary');
            $('.photo-list li p').html('');
            $('.photo-list li').slice(0, this._primaryPhotos).addClass('primary');
            $($('.photo-list li').slice(0, this._primaryPhotos)).find('p').html('Primary photo');
        },

        shoppingCartChanged: function (shoppingCart) {

            if (shoppingCart.contains('PremiumAdvert')) {
                this._maxPhotos = shoppingCart.getProduct('PremiumAdvert').Properties.NumPhotos;
                this._primaryPhotos = shoppingCart.getProduct('PremiumAdvert').Properties.PrimaryPhotos;
            }
            else if (shoppingCart.contains('PackageUpgrade')) {
                this._maxPhotos = shoppingCart.getProduct('PackageUpgrade').Properties.NumPhotos;
                this._primaryPhotos = shoppingCart.getProduct('PackageUpgrade').Properties.PrimaryPhotos;
            }
            else if (shoppingCart.contains('StandardAdvert')) {
                this._maxPhotos = shoppingCart.getProduct('StandardAdvert').Properties.NumPhotos;
                this._primaryPhotos = shoppingCart.getProduct('StandardAdvert').Properties.PrimaryPhotos;
            }
            else {
                this._maxPhotos = shoppingCart.getPaidPackage().Properties.NumPhotos;
                this._primaryPhotos = shoppingCart.getPaidPackage().Properties.PrimaryPhotos;
            }

            this.refreshCount();
            $(window).trigger('MaxPhotosChanged', this._maxPhotos);
        }
    };


    function PhotoUploader(form, options) {
        return this.initialize(form, options);
    }

    $.fn.photoUploader = function (options) {
        return new PhotoUploader(this, options);
    };

    PhotoUploader.prototype = {
        _uploadUrl: null,
        _files: {},
        _uploader: null,

        initialize: function (form, options) {

            var $this = this;

            this._uploadUrl = options.uploadUrl;

            $(function () {
                $this.createUploader();
            });
        },


        createUploader: function () {
            var $this = this;
            this._uploader = new plupload.Uploader({
                runtimes: 'html5',
                browse_button: 'pickfiles',
                max_retries: 3,
                url: $this._uploadUrl,
                filters: {
                    mime_types: [
                        { title: "Image files", extensions: "jpg,jpeg,gif,png,bmp" }
                    ]
                },
                //flash_swf_url: '/content/scripts/photo-uploader/Moxie.swf',
                //silverlight_xap_url: '/content/scripts/photo-uploader/Moxie.xap',
                init: {
                    UploadProgress: function (up, file) {
                        $this.progress(file, file.percent, file.total);
                    },
                    FilesAdded: function (up, files) {
                        plupload.each(files, function (file) {
                            $this.fileSelected(file);
                        });
                        $this._uploader.start();
                    },
                    FileUploaded: function (up, file, response) {
                        $this.fileUploaded(file, response);
                    },
                    UploadComplete: function () {
                        $this.uploadComplete();
                    },
                    Error: function (up, err) {
                        switch (err.code) {
                            case plupload.FILE_EXTENSION_ERROR:
                                $this.addFileTypeError(err.file.name);
                                break;
                            case plupload.FILE_SIZE_ERROR:
                                $this.addFileSizeError(err.file.name, err.file.total);
                                break;
                            default:
                                $this.addError("Oops! An error happened >:( #" + err.code + ": " + err.message);
                                break;
                        }
                    }
                }
            });

            this._uploader.init();
        },


        fileUploaded: function (file, data, response) {
            this._files[file.id].replaceWith(data.response);
            var id = $(data.response).data('id');
            $(window).trigger('PhotoAdded', $('li[data-id="' + id + '"]'));
        },

        fileSelected: function (file) {

            var context = $('<li class="uploading"><div class="image-container"><div class="progress progress-striped"><div class="bar"></div></div></div></li>');
            context.appendTo($('.photo-list'));

            this._files[file.id] = context;

            this.previewImage(file);
            return true;
        },

        uploadComplete: function () {
            $('a.photo_gallery').fancybox();

            this._files = {};

            $.ajax({
                url: this._submitUrl,
                type: 'POST',
                success: function (response) { }
            });
        },

        progress: function (file, percent) {
            this._files[file.id].find('.progress .bar').css({ width: percent + '%' });
        },

        validSize: function (size) {
            if (this._maxFileSize && size > this._maxFileSize) {
                return false;
            }
            return true;
        },

        cancel: function (file) {
            this._uploader.removeFile(file);
            return false;
        },

        previewImage: function (file) {
            $('<div class="file-name">Uploading <br/>' + file.name + '</div>').appendTo(this._files[file.id].find('div').first());
        },

        hideErrorContainer: function () {
            $('.error-container').addClass('hidden').find('ul').html('');
        },

        addFileTypeError: function (fileName) {
            this.addError('The file "' + fileName + '" was not recognised as an image. Please try uploading a JPG, PNG or GIF file instead.');
        },

        addFileSizeError: function (fileName, size) {
            this.addError('The file "' + fileName + '" is too large. Please stick to images under ' + this._imageSizeLimitInMB + 'MB!');
        },

        addError: function (errorMessage) {
            $('.error-container').removeClass('hidden');
            $('<li>' + errorMessage + '</li>').appendTo($('.error-container ul'));
        }
    };

})(jQuery);


