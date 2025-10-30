import { gsap } from "./gsap";
import { ScrollTrigger } from "./gsap/ScrollTrigger";
// import { ScrollSmoother } from "./gsap/ScrollSmoother";
import { ScrollToPlugin } from "./gsap/ScrollToPlugin";
import { CountUp } from "countup.js";
import lottie from 'lottie-web';
import jQuery from "jquery";
var $ = window.jQuery || jQuery;

// gsap.registerPlugin(ScrollTrigger, ScrollSmoother, ScrollToPlugin);
gsap.registerPlugin(ScrollTrigger, ScrollToPlugin);

// window.smoother = ScrollSmoother.create({
//     smooth: 0.8,
//     effects: true,
//     smoothTouch: 0,
// });

var DEFAULT_STAGGER = 0.2;
var DEFAULT_DURATION = 1;

gsap.defaults({
    ease: "power4.out",
    duration: DEFAULT_DURATION,
    stagger: DEFAULT_STAGGER,
});

gsap.registerEffect({
    name: "fadeUp",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            y: config.y,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 0,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        y: 100,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "fadeDown",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            y: config.y,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 0,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        y: -100,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "fadeLeft",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            x: config.x,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 0,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        x: 100,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "fadeRight",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            x: config.x,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 0,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        x: -100,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "fadeIn",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 0,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "fadeOut",
    effect: (targets, config) => {
        return gsap.to(targets, {
            duration: config.duration,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 0,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: 0.2,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "fadeInBlur",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            stagger: config.stagger,
            delay: config.delay,
            filter: "blur(10px)",
            opacity: 0,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "zoomIn",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            scale: config.scale,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 0,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        scale: 0.6,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "moveUp",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            y: config.y,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 1,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        y: 100,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "slideMoveUp",
    effect: (targets, config) => {
        return gsap.set(targets, {
            y: config.y,
            opacity: 0,
            autoAlpha: 0,
            onComplete: function () {
                gsap.to(targets, {
                    duration: config.duration,
                    y: 0,
                    stagger: config.stagger,
                    delay: config.delay,
                    opacity: 1,
                    autoAlpha: 1,
                    onComplete: config.onComplete,
                });
            },
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        y: 100,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "lineX",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            scaleX: 0,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 1,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: 1,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "flipInX",
    effect: (targets, config) => {
        return gsap.from(targets, {
            duration: config.duration,
            rotateX: 90,
            stagger: config.stagger,
            delay: config.delay,
            opacity: 1,
            onComplete: config.onComplete,
        });
    },
    defaults: {
        duration: 1.6,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "textfadeLeft",
    effect: (targets, config) => {
        for (let i = 0; i < targets.length; i++) {
            // if (!targets[i].dataset.plaintext) {
            //     targets[i].dataset.plaintext = targets[i].innerText;
            // }

            // targets[i].innerHTML = targets[i].dataset.plaintext.split(' ').map(function (c) {
            //     var hyphens = c.split('-').map((hyphen) => {
            //         var word = hyphen.split('').map((s) => ('<div class="span" style="display: inline-block;">' + s + '</div>')).join('');
            //         return '<div style="display: inline-block;">' + word + '</div>';
            //     }).join('<div class="span" style="display: inline-block;">-</div>');
            //     return '<div style="display: inline-block; margin-right: 0.2em;">' + hyphens + '</div>';

            //     // var word = c.split('').map((s) => ('<span>' + s + '</span>')).join('');
            //     // return '<div style="display: inline-block; margin-right: 0.2em;">' + word + '</div>';
            // }).join('');

            if (!targets[i].dataset.html) {
                targets[i].dataset.html = targets[i].innerHTML;
            }

            var words = targets[i].dataset.html.split(/(<[^>]+>|\s+)/g).filter(segment => segment !== '');
            var html = '';

            for (let j = 0; j < words.length; j++) {
                const word = words[j];
                if (word == ' ') {
                    html += '<div class="span" style="display: inline-block;">&nbsp;</div>';
                } else if (word.indexOf('<') == 0) {
                    html += word;
                } else {
                    var hyphens = word.split('-').map((hyphen) => {
                        var w = hyphen.split('').map((s) => ('<div class="span" style="display: inline-block;">' + s + '</div>')).join('');
                        return '<div style="display: inline-block;">' + w + '</div>';
                    }).join('<div class="span" style="display: inline-block;">-</div>');
                    html += ('<div style="display: inline-block;">' + hyphens + '</div>');
                }
            }

            targets[i].innerHTML = html;
        }

        return targets.forEach((el) => {
            gsap.timeline({ scrollTrigger: { trigger: el, start: "top bottom", toggleActions: "play none none none" } }).from(el.querySelectorAll('.span'), {
                duration: 0.3,
                stagger: 0.03,
                delay: config.delay,
                x: 30,
                opacity: 0,
            });
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        x: 50,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "textfadeLeft2",
    effect: (targets, config) => {
        for (let i = 0; i < targets.length; i++) {
            if (!targets[i].dataset.html) {
                targets[i].dataset.html = targets[i].innerHTML;
            }

            var words = targets[i].dataset.html.split(/(<[^>]+>|\s+)/g).filter(segment => segment !== '');
            var html = '';
            var classes = targets[i].dataset.classes || '';

            for (let j = 0; j < words.length; j++) {
                const word = words[j];
                if (word == ' ') {
                    html += '<div class="span" style="display: inline-block;">&nbsp;</div>';
                } else if (word.indexOf('<') == 0) {
                    html += word;
                } else {
                    var hyphens = word.split('-').map((hyphen) => {
                        var w = hyphen.split('').map((s) => ('<div class="span ' + classes + '" style="display: inline-block;">' + s + '</div>')).join('');
                        return '<div style="display: inline-flex;">' + w + '</div>';
                    }).join('<div class="span ' + classes + '" style="display: inline-block;">-</div>');
                    html += ('<div style="display: inline-block;">' + hyphens + '</div>');
                }
            }

            targets[i].innerHTML = html;
        }

        return targets.forEach((el) => {
            gsap.timeline({ scrollTrigger: { trigger: el, start: "top bottom", toggleActions: "play none none none" } }).from(el.querySelectorAll('.span'), {
                duration: 1,
                stagger: 0.05,
                delay: config.delay,
                scale: 1.2,
                opacity: 0,
            });
        });
    },
    defaults: {
        duration: DEFAULT_DURATION,
        stagger: DEFAULT_STAGGER,
        delay: 0,
        x: 50,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "countUp",
    effect: (targets, config) => {
        return targets.forEach((el) => {
            //toggleActions: "restart none none reverse"
            gsap.timeline({ scrollTrigger: { trigger: el, start: "top bottom", toggleActions: "play none none none" } }).set(el, {
                opacity: 1,
                autoAlpha: 1,
                duration: 0,
                delay: config.delay,
                onComplete: function () {
                    var numAnim = new CountUp(el, parseFloat(el.getAttribute("data-num")), {
                        duration: config.duration,
                        separator: el.getAttribute("data-num-separator") || '',
                        decimalPlaces: el.getAttribute("data-num-decimal") || 0
                    });
                    numAnim.start();
                },
            });
        });
    },
    defaults: {
        duration: 2,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "scrollNumber",
    effect: (targets, config) => {
        for (let i = 0; i < targets.length; i++) {
            var el = targets[i];
            $(el).attr('data-num', el.innerHTML);
            Object.assign(el.style, {
                position: "relative",
                overflow: "hidden",
                height: el.offsetHeight + 'px',
            });
            const digits = el.dataset.num.toString().split('');
            var html = '';
            for (let j = 0; j < digits.length; j++) {
                if (digits[j] == '+') {
                    html += '<span>+</span>';
                } else {
                    const spanList = Array(10).join(0).split(0).map((x, num) => `<span>${num}</span>`).join('');
                    html += `<span style="transform: translateY(-${5 * parseInt(digits[j])}%); display: inline-flex; text-align: center; flex-direction: column; opacity: 0;" data-value="${digits[j]}">${spanList + spanList}</span>`;
                }
            }
            $(el).html(html);
        }
        return targets.forEach((el) => {
            gsap.timeline({ scrollTrigger: { trigger: el, start: "top bottom", toggleActions: "play none none none" } }).set(el, {
                opacity: 1,
                autoAlpha: 1,
                duration: config.duration,
                delay: config.delay,
                onComplete: function () {
                    el.querySelectorAll('span[data-value]').forEach((tick, i) => {
                        gsap.to(tick, {
                            opacity: 1,
                            autoAlpha: 1,
                            y: `-${5 * (10 + parseInt(tick.dataset.value))}%`,
                            duration: 5,
                            delay: 0.2 * i,
                        });
                    });
                },
            });
        });
    },
    defaults: {
        duration: 2,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

gsap.registerEffect({
    name: "boxX",
    effect: (targets, config) => {
        targets.forEach((el) => {
            $(el).children().first().width($(el).parent().width());
            $(el).width(0);
            $(el).css({ opacity: 0, overflow: 'hidden' });
        });
        return targets.forEach((el) => {
            gsap.timeline({ scrollTrigger: { trigger: el, start: "top bottom", toggleActions: "play none none none" } })
                .set(el.querySelectorAll('img'), {
                    duration: 0,
                    opacity: 0,
                    autoAlpha: 1,
                    scale: 1.2,
                    delay: config.delay,
                    onComplete: function () {
                        gsap.to(el.querySelectorAll('img'), {
                            duration: 1,
                            opacity: 1,
                            autoAlpha: 1,
                            scale: 1,
                        });
                    }
                })
            gsap.timeline({ scrollTrigger: { trigger: el, start: "top bottom", toggleActions: "play none none none" } }).to(el, {
                duration: 1,
                width: '100%',
                opacity: 1,
                autoAlpha: 1,
                delay: config.delay
            });
        });
    },
    defaults: {
        duration: 1,
        delay: 0,
        onComplete: function () { },
    },
    extendTimeline: true,
});

// gsap.registerEffect({
//     name: "boxShadow",
//     effect: (targets, config) => {
//         return gsap.from(targets, {
//             duration: config.duration,
//             stagger: config.stagger,
//             delay: config.delay,
//             opacity: 0,
//             x: -10,
//             y: -10,
//             onComplete: config.onComplete,
//         });
//     },
//     defaults: {
//         duration: 1,
//         stagger: 0.1,
//         delay: 0,
//         onComplete: function () { },
//     },
//     extendTimeline: true,
// });

function runSecondaryAnimation(fromEl) {
    fromEl.querySelectorAll('[data-sub-ani]').forEach((el) => {
        var gsapAni = el.getAttribute('data-sub-ani');
        var gsapTarget = el.getAttribute('data-target');
        var gsapDelay = el.getAttribute('data-delay');

        let delay = 0.2;
        if (gsapDelay) delay += parseFloat(gsapDelay);

        if (gsapAni) {
            gsap.timeline()[gsapAni](gsapTarget ? fromEl.querySelectorAll(gsapTarget) : el, { delay: delay });
        }
    });
}

function createAnime(el, gsapAni) {
    let gsapTarget = $(el).attr("data-target");
    let gsapDelay = $(el).attr("data-delay");
    let gsapDelayLg = $(el).attr("data-delay-lg");
    let gsapStagger = $(el).attr('data-stagger');
    let sequence = $(el).attr('data-sequence');

    let delay = 0;
    if (gsapDelayLg && window.innerWidth >= 1024) delay = parseFloat(gsapDelayLg);
    else if (gsapDelay) delay = parseFloat(gsapDelay);

    if (gsapAni) {
        if (gsapTarget && window.innerWidth <= 768 && sequence != 'true') {
            var allTargets = $(el).find(gsapTarget);
            if (allTargets.length) {
                for (let i = 0; i < allTargets.length; i++) {
                    gsap.timeline({
                        scrollTrigger: {
                            trigger: allTargets[i],
                            start: "top 100%",
                            // toggleActions: "restart none none reverse",
                        },
                    })[gsapAni](allTargets[i], {
                        delay: 0.4,
                        stagger: {
                            each: gsapStagger ? parseFloat(gsapStagger) : DEFAULT_STAGGER,
                            onStart: function () {
                                runSecondaryAnimation(this.targets()[0]);
                            }
                        }
                    });
                }
                return true;
            }
        }
        gsap.timeline({
            scrollTrigger: {
                trigger: $(el),
                start: "top 80%",
                // toggleActions: "restart none none reverse",
            },
        })[gsapAni](gsapTarget ? $(el).find(gsapTarget) : $(el), {
            delay: delay,
            stagger: {
                each: gsapStagger ? parseFloat(gsapStagger) : DEFAULT_STAGGER,
                onStart: function () {
                    runSecondaryAnimation(this.targets()[0]);
                }
            }
        });
    }
}

function createScrollToAnime() {
    var headerTop = ($('#header-wrap').outerHeight() || 0) + ($('#wpadminbar').outerHeight() || 0);

    document.querySelectorAll('[data-anime="scroll-to"]').forEach((el) => {
        var target = el.getAttribute("data-target");
        if (target) {
            el.addEventListener("click", function () {
                gsap.to(window, { scrollTo: { y: target, offsetY: headerTop } });
            });
        }
    });

    const handleHashChange = function () {
        var hash = window.location.hash;
        if (hash && document.getElementById('section-' + hash.slice(1))) {
            gsap.to(window, { scrollTo: { y: document.getElementById('section-' + hash.slice(1)), offsetY: headerTop } })
        };
    }

    setTimeout(() => {
        handleHashChange();
    }, 1000);
    window.addEventListener("hashchange", handleHashChange, false);

    document.querySelectorAll('[data-anime="scroll-to-top"]').forEach((el) => {
        el.addEventListener("click", function () {
            gsap.to(window, { scrollTo: { y: 0 } });
        });
    });
}

function toggleScrolling() {
    let scrollTimer;
    $(window).on("scroll", function () {
        $('.toggle-scrolling').addClass("active");
        clearTimeout(scrollTimer);
        scrollTimer = setTimeout(function () {
            $('.toggle-scrolling').removeClass("active");
        }, 800);
    });
}

function startAnimation() {
    $("[data-ani]").each(function (idx, el) {
        let gsapAni = $(el).attr("data-ani");
        createAnime(el, gsapAni);
    });

    $("[data-ani-md]").each(function (idx, el) {
        if (window.innerWidth <= 768) {
            let gsapAni = $(el).attr("data-ani-md");
            createAnime(el, gsapAni);
        }
    });

    $("[data-ani-lg]").each(function (idx, el) {
        if (window.innerWidth > 768 && window.innerWidth <= 1280) {
            let gsapAni = $(el).attr("data-ani-lg");
            createAnime(el, gsapAni);
        }
    });

    $("[data-ani-xl]").each(function (idx, el) {
        if (window.innerWidth > 1280) {
            let gsapAni = $(el).attr("data-ani-xl");
            createAnime(el, gsapAni);
        }
    });

    $('[data-parallax-xl="lag"]').each(function (idx, el) {
        if (window.innerWidth > 1280) {
            gsap.set(el, { y: -60, opacity: 1, autoAlpha: 1 });
            gsap.timeline({
                scrollTrigger: {
                    trigger: el, start: "top bottom", end: "center top", onUpdate: function (scroll) {
                        gsap.to(el, { y: (120 * scroll.progress) - 60, ease: "power2.out", duration: 2, delay: 0.3, opacity: 1, autoAlpha: 1 });
                    }
                }
            });
        }
    });

    $('[data-parallax]').each(function (idx, el) {
        gsap.timeline().fromTo(el, {
            y: "-30vh"
        }, {
            y: "30vh",
            data: "gsap-inner",
            scrollTrigger: {
                trigger: $(el).parent(),
                scrub: !0,
                start: "top bottom"
            },
            ease: "none"
        });
    });

    createScrollToAnime();
    toggleScrolling();

    $(".timeline").each(function (idx, el) {
        // var elHeight = $(el).height();
        ScrollTrigger.create({
            trigger: el,
            start: "top 50%",
            end: "bottom 50%",
            // onEnter: () => console.log("Entered!"),
            // onLeave: () => console.log("Left!"),
            onUpdate: self => {
                // $('.timeline-progress').height((self.progress * 100) + '%');

                var line = (self.end - self.start) * self.progress;
                $('.timeline-progress').height(line + 'px');

                $('.timeline-item-point').each((i, point) => {
                    var rect = point.getBoundingClientRect();
                    var absoluteTop = rect.top - document.querySelector('.timeline').getBoundingClientRect().top;
                    var parent = $(point).closest('.timeline-item');
                    var info = parent.find('.timeline-item-info');
                    var img = parent.find('.timeline-item-img');

                    if (line >= absoluteTop) {
                        if (!parent.hasClass('loaded')) {
                            gsap.timeline().set(info, {
                                opacity: 1,
                                autoAlpha: 1,
                                duration: 0,
                                onComplete: function () {
                                    gsap.timeline().boxX(img);
                                    gsap.timeline().fadeUp(info);
                                }
                            });
                            parent.addClass('loaded');
                        }
                    } else {
                        gsap.timeline().fadeOut(img, { duration: 1 });
                        gsap.timeline().fadeOut(info, { duration: 1 });
                        parent.removeClass('loaded');
                    }
                });

            }
        });
    });

    $('.lottie-ani').each(function (idx, el) {
        var animation = lottie.loadAnimation({
            container: el,
            renderer: 'svg',
            autoplay: false,
            path: el.dataset.src
        });
        // var container = $(el).closest('.lottie-ani-container');
        // ScrollTrigger.create({
        //     trigger: container.children()[0],
        //     start: "top top",
        //     end: "bottom+=2000 top", // extend scroll distance
        //     scrub: true, // smooth link between scroll & animation
        //     pin: true, // makes it sticky
        //     onUpdate: self => {
        //         if (animation.totalFrames > 0) {
        //             const frame = self.progress * animation.totalFrames;
        //             // console.log(frame, animation.totalFrames);
        //             if(frame < animation.totalFrames) {
        //                 animation.goToAndStop(frame, true);
        //             }
        //         }
        //     }
        // });
        ScrollTrigger.create({
            trigger: el,
            start: "top bottom",
            end: "bottom 80%",
            onUpdate: self => {
                if (animation.totalFrames > 0) {
                    const frame = self.progress * animation.totalFrames;
                    if (frame < animation.totalFrames) {
                        animation.goToAndStop(frame, true);
                    }
                }
            }
        });
    });
}

export { gsap, startAnimation };
