<?php

/**
 * Template Name: Raymag Template - Thank you
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
        <div class="relative z-10 max-w-[768px] mx-auto px-3 lg:px-10 text-center py-[200px]">
            <h1 class="ani-item text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-primary-300"><?php the_title(); ?></h1>
        </div>
    </div>
    
    <div class="max-w-[768px] mx-auto px-3 lg:px-10 text-center py-[100px]">
        <h6 class="ani-item text-[16px] lg:text-[18px] leading-175 text-gray-100"><?php the_custom_field('thank_content'); ?></h6>
        <div class="ani-item mt-10">
            <a href="<?php echo esc_url(home_url('/')); ?>" class="inline-block rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative">
                <div class="inline-flex items-center gap-x-[10px] px-10 py-[10px] relative">
                    <div class="text-[16px] leading-175 font-medium text-white"><?php the_trans('返回首頁'); ?></div>
                </div>
            </a>
        </div>
    </div>
</div>
<?php
get_footer();
