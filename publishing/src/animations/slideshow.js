/** Direction constants */
const NEXT = 1;
const PREV = -1;

/**
 * Slideshow Class
 * Manages slideshow functionality including navigation and animations.
 * @export
 */
export class Slideshow {

	/**
	 * Holds references to relevant DOM elements.
	 * @type {Object}
	 */
	DOM = {
		el: null,            // Main slideshow container
		slides: null,        // Individual slides
		slidesInner: null,    // Inner content of slides (usually images)
		gsap: null,
		currentNumEl: null,
		contents: null,
	};
	/**
	 * Index of the current slide being displayed.
	 * @type {number}
	 */
	current = 0;
	/**
	 * Total number of slides.
	 * @type {number}
	 */
	slidesTotal = 0;

	/**  
	 * Flag to indicate if an animation is running.
	 * @type {boolean}
	 */
	isAnimating = false;

	/**
	 * Slideshow constructor.
	 * Initializes the slideshow and sets up the DOM elements.
	 * @param {HTMLElement} DOM_el - The main element holding all the slides.
	 */
	constructor(DOM_el, gsap) {
		// Initialize DOM elements
		this.DOM.el = DOM_el;
		this.DOM.slides = [...this.DOM.el.querySelectorAll('.slide')];
		this.DOM.contents = [...this.DOM.el.querySelectorAll('.slide-content')];
		this.DOM.slidesInner = this.DOM.slides.map(item => item.querySelector('.slide__img'));

		// Set initial slide as current
		this.DOM.slides[this.current].classList.add('slide--current');

		// Count total slides
		this.slidesTotal = this.DOM.slides.length;
		this.DOM.el.querySelector('.slides-total__num').innerHTML = this.slidesTotal;

		this.DOM.currentNumEl = this.DOM.el.querySelector('.slides-current__num');
		this.DOM.currentNumEl.innerHTML = this.current + 1;

		this.DOM.gsap = gsap;
		this.addNav();
	}

	addNav() {
		this.DOM.el.querySelector('.slides-nav__item--prev').addEventListener('click', () => this.prev());
		this.DOM.el.querySelector('.slides-nav__item--next').addEventListener('click', () => this.next());
	}

	/**
	 * Navigate to the next slide.
	 * @returns {void}
	 */
	next() {
		this.navigate(NEXT);
	}

	/**
	 * Navigate to the previous slide.
	 * @returns {void}
	 */
	prev() {
		this.navigate(PREV);
	}

	/**
	 * Navigate through slides.
	 * @param {number} direction - The direction to navigate. 1 for next and -1 for previous.
	 * @returns {boolean} - Return false if the animation is currently running.
	 */
	navigate(direction) {
		// Check if animation is already running
		if (this.isAnimating) return false;
		this.isAnimating = true;

		// Update the current slide index based on direction
		const previous = this.current;
		this.current = direction === 1 ?
			this.current < this.slidesTotal - 1 ? ++this.current : 0 :
			this.current > 0 ? --this.current : this.slidesTotal - 1

		// Get the current and upcoming slides and their inner elements
		const currentSlide = this.DOM.slides[previous];
		const currentInner = this.DOM.slidesInner[previous];
		const upcomingSlide = this.DOM.slides[this.current];
		const upcomingInner = this.DOM.slidesInner[this.current];

		this.DOM.currentNumEl.innerHTML = this.current + 1;

		// Animation sequence using GSAP
		this.DOM.gsap
			.timeline({
				defaults: {
					duration: 1.6,
					ease: 'power3.inOut'
				},
				onStart: () => {
					// Add class to the upcoming slide to mark it as current
					this.DOM.slides[this.current].classList.add('slide--current');
				},
				onComplete: () => {
					// Remove class from the previous slide to unmark it as current
					this.DOM.slides[previous].classList.remove('slide--current');
					// Reset animation flag
					this.isAnimating = false;
				}
			})
			// Defining animation steps
			.addLabel('start', 0)
			.to(currentSlide, {
				xPercent: -direction * 100
			}, 'start')
			.to(currentInner, {
				startAt: { transformOrigin: direction === NEXT ? '100% 50%' : '0% 50%' },
				scaleX: 4
			}, 'start')
			.fromTo(upcomingSlide, {
				xPercent: direction * 100
			}, {
				xPercent: 0
			}, 'start')
			.fromTo(upcomingInner, {
				transformOrigin: direction === NEXT ? '0% 50%' : '100% 50%',
				xPercent: -direction * 100,
				scaleX: 4
			}, {
				xPercent: 0,
				scaleX: 1
			}, 'start');

		// slide content animation
		const currentContent = this.DOM.contents[previous];
		const upcomingContent = this.DOM.contents[this.current];
		this.DOM.gsap.to(currentContent, {
			opacity: 0,
			duration: 1,
			onComplete: () => {
				currentContent.style.display = 'none';

				upcomingContent.style.display = '';
				upcomingContent.style.opacity = 1;
				upcomingContent.querySelectorAll('[data-ani]').forEach((el) => {
					var gsapAni = el.getAttribute('data-ani');
					var gsapTarget = el.getAttribute('data-target');
					var gsapDelay = el.getAttribute('data-delay');

					let delay = 0;
					if (gsapDelay) delay = parseFloat(gsapDelay);

					if (gsapAni) {
						this.DOM.gsap.timeline()[gsapAni](gsapTarget ? upcomingContent.querySelectorAll(gsapTarget) : el, { delay: delay });
					}
				});
			}
		});
	}

}