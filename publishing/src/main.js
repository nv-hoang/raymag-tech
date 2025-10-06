import './style.css';
import Alpine from 'alpinejs';
import jQuery from 'jquery';
import { Fancybox } from "@fancyapps/ui";
import "@fancyapps/ui/dist/fancybox/fancybox.css";
import { gsap, startAnimation } from "./animations/animations";
import { SwiperInit } from './animations/swiper';
// import { FormInit } from './animations/form';
import axios from 'axios';

var $ = window.$ = window.jQuery || jQuery;

function start() {
    startAnimation();
    Fancybox.bind("[data-fancybox]", {
        Slideshow: {
            playOnStart: true,
        }
    });

    SwiperInit(gsap);
    // FormInit();

    Alpine.data('languages', () => ({
        changeToLang(lang) {
            const url = new URL(window.location.href);
            url.searchParams.set('lang', lang);
            window.location.href = url.toString();
        }
    }));

    Alpine.data('frmdata', () => ({
        action: '',
        formdata: {},
        loading: false,
        errors: {},
        success: false,
        init() {
            this.action = this.$el.dataset.action;
            this.formdata = JSON.parse(atob(this.$el.dataset.formfields));
        },
        dosubmit() {
            if(this.loading) return;
            this.loading = true;
            this.errors = {};
            
            axios.post(this.action, {
                FormData: JSON.stringify(this.formdata),
                SubmitAction: "send",
            }).then(res => {
                this.loading = false;
                if(res.data.status == "Success") {
                    this.success = true;
                } else if(res.data.errors) {
                    this.errors = res.data.errors;
                }
            });
        },
    }));

    Alpine.start();
}

$(function () {
    var ploading = $('#ploading div');
    if (ploading.length) {
        start();
        gsap.to(ploading.get(0).querySelectorAll('.ani-item-1'), { opacity: 1, autoAlpha: 1, delay: 0.4 });
        gsap.to(ploading.get(0).querySelectorAll('.ani-item-2'), { width: '134px', opacity: 1, autoAlpha: 1, delay: 0.8 });
        gsap.to(ploading.get(0), { opacity: 0, autoAlpha: 0, delay: 1.4, duration: 2 });
    } else {
        start();
    }
});