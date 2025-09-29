import jQuery from 'jquery';
import Swiper from 'swiper/bundle';
import 'swiper/css/bundle';

var $ = window.jQuery || jQuery;

export function SwiperInit(gsap) {
    function slideAni(el, swiper) {
        // var items = el.querySelectorAll(`.swiper-slide-${swiper.realIndex} .ani-item`);
        // if (el.dataset.slideAni && items.length) gsap.timeline()[el.dataset.ani](items);
        var items = el.querySelectorAll(`.swiper-slide-${swiper.realIndex} [data-ani]`);
        for (let i = 0; i < items.length; i++) {
            var target = items[i].dataset.target;
            var delay = 0;
            if (items[i].dataset.delay) {
                delay = parseFloat(items[i].dataset.delay);
            }
            gsap.timeline()[items[i].dataset.ani](target ? items[i].querySelectorAll(target) : items[i], { delay: delay });
        }

        if (el.dataset.caption) {
            $(el.dataset.caption).hide();
            var caption = document.querySelector(`${el.dataset.caption}-${swiper.realIndex}`);
            if (caption) {
                caption.style.display = '';
                caption.style.opacity = 1;
                if (el.dataset.slideAni) gsap.timeline()[el.dataset.slideAni](caption.querySelectorAll('.ani-item'), { delay: 0.4 });
            }
        }
    }

    function initSwiper(el) {
        var instance = $(el).data('swiperInstance');
        if (instance) instance.destroy();

        if ((el.dataset.xl == undefined || (el.dataset.xl == 'false' && window.innerWidth <= 1280)) && (el.dataset.xxl == undefined || (el.dataset.xxl == 'false' && window.innerWidth < 1600))) {
            $(el).addClass('swiper');
            $(el).children().first().addClass('swiper-wrapper');

            if (el.dataset.thumb) {
                var thumbEl = $(el.dataset.thumb)[0];

                var thumb = new Swiper(el.dataset.thumb, {
                    spaceBetween: thumbEl.dataset.spaceBetween || 8,
                    slidesPerView: thumbEl.dataset.slidesPerView || 4,
                    freeMode: true,
                    watchSlidesProgress: true,
                });
            }

            var swiperInstance = new Swiper(el, {
                loop: el.dataset.loop == 'true',
                effect: el.dataset.effect || 'slide',
                speed: el.dataset.effectSpeed || 300,
                spaceBetween: el.dataset.spaceBetween || 0,
                slidesPerView: el.dataset.slidesPerView || 1,
                breakpoints: el.dataset.breakpoints ? JSON.parse(el.dataset.breakpoints) : null,
                centeredSlides: el.dataset.centeredSlides == 'true',
                allowTouchMove: el.dataset.allowTouchMove != 'false',
                autoplay: el.dataset.autoplay ? {
                    delay: el.dataset.autoplay != 'true' ? parseInt(el.dataset.autoplay) : 3000,
                    disableOnInteraction: false,
                } : false,
                navigation: {
                    nextEl: el.dataset.next ? document.querySelector(el.dataset.next) : el.querySelector('.swiper-next'),
                    prevEl: el.dataset.prev ? document.querySelector(el.dataset.prev) : el.querySelector('.swiper-prev'),
                },
                pagination: {
                    el: el.dataset.pagination ? document.querySelector(el.dataset.pagination) : el.querySelector('.swiper-pagination'),
                    clickable: true,
                },
                thumbs: { swiper: thumb || null },
                on: {
                    afterInit: function (swiper) {
                        // slideAni(el, swiper);
                        setTimeout(() => {
                            if (el.dataset.index) {
                                $(el.dataset.index).text(String(swiper.realIndex + 1).padStart(2, '0'));
                            }
                            if (el.dataset.total) {
                                $(el.dataset.total).text(String(swiper.slides.length).padStart(2, '0'));
                            }

                            el.dispatchEvent(new CustomEvent('swiperinit', {
                                bubbles: true, detail: {
                                    total: swiper.slides.length,
                                    progress: Math.round(((swiper.realIndex + 1) * 100) / swiper.slides.length) + '%'
                                }
                            }));

                            if (el.dataset.initcenter) {
                                swiper.slideNext();
                            }
                        }, 400);
                    },
                    slideChangeTransitionStart: function (swiper) {
                        slideAni(el, swiper);
                        setTimeout(() => {
                            if (el.dataset.index) {
                                $(el.dataset.index).text(String(swiper.realIndex + 1).padStart(2, '0'));
                            }

                            el.dispatchEvent(new CustomEvent('swiperchange', {
                                bubbles: true, detail: {
                                    currentSlide: swiper.realIndex + 1,
                                    progress: Math.round(((swiper.realIndex + 1) * 100) / swiper.slides.length) + '%'
                                }
                            }));
                        }, 0);
                    },
                },
            });

            $(el).data('swiperInstance', swiperInstance);

            if (el.dataset.thumb) {
                $(el.dataset.thumb).find('.swiper-slide').on('mouseenter', function () {
                    swiperInstance.slideTo($(this).index());
                });
            }
        } else {
            $(el).removeClass('swiper');
            $(el).children().first().removeClass('swiper-wrapper');
        }
    }

    var refreshSwiper = function () {
        $('.swiper-ani').each(function (idx, el) {
            if ($(el).data('xl') !== undefined) {
                initSwiper(el);
            } else {
                var _swiperInstance = $(el).data('swiperInstance');
                if (_swiperInstance) _swiperInstance.update();
            }
        });
    }
    // init();
    window.addEventListener("initswiper", function (e) {
        initSwiper(e.target);
    });
    window.addEventListener("resize", refreshSwiper);
}