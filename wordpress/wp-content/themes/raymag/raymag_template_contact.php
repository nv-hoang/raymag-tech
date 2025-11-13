<?php

/**
 * Template Name: Raymag Template - Contact
 * Template Post Type: page
 *
 * This is the most generic template file in a WordPress theme
 * and one of the two required files for a theme (the other being style.css).
 * It is used to display a page when nothing more specific matches a query.
 * E.g., it puts together the home page when no home.php file exists.
 *
 * @link https://developer.wordpress.org/themes/basics/template-hierarchy/
 *
 * @package raymag
 */

get_header();
?>

<div data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <div class="relative">
        <img src="<?php the_theme_asset_url('assets/img/bg-contact-heading.webp'); ?>" class="absolute inset-0 w-full h-full object-cover">
        <div class="absolute inset-0 w-full h-full" style="background: linear-gradient(180deg, rgba(3, 10, 17, 0.5) 0%, #030A11 100%);"></div>
        <div class="relative z-10 max-w-[768px] mx-auto px-3 lg:px-10 text-center py-[100px] lg:py-[200px]">
            <h1 class="ani-item text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-primary-300"><?php the_title(); ?></h1>
        </div>
    </div>

    <div class="max-w-[768px] mx-auto px-3 lg:px-10 pb-[100px]">
        <div class="ani-item editor-content-style text-[16px] lg:text-[18px] leading-175 text-gray-100 text-center">
            <?php the_content(); ?>
        </div>

        <div class="ani-item mt-10">
            <?php if ($form = get_custom_field('contact_form')): ?>
                <p class="text-right text-[16px] leading-175 text-primary-300 mb-7">*為必填欄位</p>
                <?php echo do_shortcode('[wpforms id="' . $form->ID . '"]'); ?>
            <?php endif; ?>
        </div>
    </div>
</div>
<style>
    ::-webkit-input-placeholder {
        color: var(--placeholder-color, transparent) !important;
        opacity: 1 !important;
    }

    ::-moz-placeholder {
        color: var(--placeholder-color, transparent) !important;
        opacity: 1 !important;
    }

    :-ms-input-placeholder {
        color: var(--placeholder-color, transparent) !important;
        opacity: 1 !important;
    }

    :-moz-placeholder {
        color: var(--placeholder-color, transparent) !important;
        opacity: 1 !important;
    }

    div.wpforms-container-full:not(:empty) {
        margin: 0px !important;
        padding: 0px !important;
    }

    .wpforms-field-medium {
        background: #030A11E5 !important;
        backdrop-filter: blur(20px) !important;
        outline: none !important;
        line-height: 175% !important;
        font-size: 16px !important;
        padding-top: 10px !important;
        padding-bottom: 10px !important;
        padding-left: .75rem !important;
        padding-right: .75rem !important;
        border-width: 1px !important;
        border-radius: 28px !important;
        --tw-border-opacity: .2;
        border-color: rgb(160 205 228 / var(--tw-border-opacity, 1)) !important;
        width: 100% !important;
        max-width: 100% !important;
        box-shadow: none !important;
        font-weight: 500 !important;
        color: #A0CDE4 !important;
    }

    .wpforms-field-medium:not(textarea) {
        height: 48px !important;
    }

    .wpforms-field-medium:focus {
        --tw-text-opacity: 1;
        color: rgb(255 255 255 / var(--tw-text-opacity, 1)) !important;
    }

    .wpforms-error {
        margin: 0px !important;
        margin-top: 2px !important;
        color: #ff7e7e !important;
    }

    .wpforms-error::before {
        display: none !important;
    }

    label.wpforms-field-label {
        font-size: 16px !important;
        font-weight: 400 !important;
        color: #fff !important;
        line-height: 175% !important;
        margin-bottom: .25rem !important;
    }

    label.wpforms-field-label>span {
        color: #fff !important;
    }

    .wpforms-field:not(.wpforms-field-layout) {
        padding-top: 14px !important;
        padding-bottom: 14px !important;
    }

    .wpforms-submit-container {
        margin-top: 14px !important;
        border-radius: 9999px !important;
        overflow: hidden !important;
        opacity: 1 !important;
        display: block !important;
        width: 100% !important;
        position: relative;
        background: linear-gradient(90deg, #00D5E9 0%, #0895DA 100%) !important;
    }

    .wpforms-submit-container::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        width: 200%;
        height: 100%;
        background: linear-gradient(90deg, #0895DA 0%, #00D5E9 50%, #0895DA 100%);
        transform: translateX(-50%);
        transition: transform 300ms ease-in-out;
        border: 0px !important;
        outline: none !important;
        box-shadow: none !important;
    }

    .wpforms-submit-container:hover::before {
        transform: translateX(0%);
    }

    .wpforms-submit {
        background: none !important;
        border: none !important;
        outline: none !important;
        box-shadow: none !important;
        opacity: 1 !important;
        position: relative;
        padding: 10px !important;
        color: #FFFFFF !important;
        font-size: 16px !important;
        line-height: 175% !important;
        font-weight: 500 !important;
        text-align: center !important;
        display: block !important;
        width: 100% !important;
        height: 48px !important;
    }

    .wpforms-submit-spinner {
        position: absolute !important;
        z-index: 1 !important;
        top: 11px !important;
        right: 12px !important;
    }

    .wpforms-confirmation-container-full {
        text-align: center !important;
    }
</style>
<?php
get_footer();
