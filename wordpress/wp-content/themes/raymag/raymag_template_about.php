<?php

/**
 * Template Name: Raymag Template - About
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
<div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item" data-sequence="true">
    <div data-ani="fadeUp" data-delay="0.6" data-target=".ani-item-2" class="relative" data-sequence="true">
        <div data-ani="fadeUp" data-delay="1.4" data-target=".ani-item-3" class="relative" data-sequence="true">
            <div class="relative h-[880px]">
                <img src="<?php the_custom_field('about_kv.aboutkv_imagemobile'); ?>" class="lg:hidden absolute inset-0 w-full h-full object-cover pointer-events-none">
                <img src="<?php the_custom_field('about_kv.aboutkv_image'); ?>" class="hidden lg:block absolute inset-0 w-full h-full object-cover pointer-events-none">

                <div class="relative z-10 2xl:max-w-[1680px] mx-auto">
                    <div class="lg:max-w-[600px] 2xl:max-w-[800px]">
                        <div class="px-3 lg:px-10 pt-[100px] lg:pt-[223px] 2xl:pt-[283px]">
                            <?php
                            $title = explode(' ', get_custom_field('about_kv.aboutkv_title'), 2);
                            ?>
                            <div style="font-family: 'Inter';" class="flex flex-col flex-wrap md:flex-row md:gap-x-5 lg:flex-col 2xl:flex-row">
                                <div>
                                    <div class="ani-item text-[40px] leading-125 font-semibold text-white lg:text-[88px] lg:leading-100"><?php echo $title[0]; ?></div>
                                </div>
                                <div class="overflow-hidden">
                                    <div class="ani-item-2 gradient-1 text-[40px] leading-125 font-semibold lg:text-[88px] lg:leading-100"><?php echo (count($title) > 1 ? $title[1] : ''); ?></div>
                                </div>
                            </div>

                            <h1 class="ani-item-3 text-[32px] lg:text-[40px] leading-150 lg:leading-125 font-semibold lg:font-medium text-white mt-7"><?php the_custom_field('about_kv.aboutkv_subtitle'); ?></h1>
                            <div class="ani-item-3 text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5">
                                <?php the_custom_field('about_kv.aboutkv_desc'); ?>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="max-w-[1680px] mx-auto px-3 lg:px-10 py-[100px]">
    <div class="lottie-ani-container">
        <div>
            <div class="flex items-center flex-col lg:flex-row gap-10">
                <div class="xl:w-[880px] lg:w-[70%]">
                    <div class="relative">
                        <div class="lottie-ani" data-src="<?php the_custom_field('about_info.aboutinfo_lottiejson'); ?>"></div>
                        <div class="absolute inset-0 w-full h-full" style="background: radial-gradient(50% 50% at 50% 50%, rgba(3, 10, 17, 0) 55.83%, #030A11 100%);"></div>
                    </div>
                </div>
                <div class="lg:px-10 flex-1" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item" data-sequence="true">
                    <div class="ani-item text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300"><?php the_custom_field('about_info.aboutinfo_title'); ?></div>
                    <div class="ani-item text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5"><?php the_custom_field('about_info.aboutinfo_desc'); ?></div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="max-w-[1680px] mx-auto px-3 lg:px-10 py-[100px]">
    <div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item" data-sequence="true">
        <div data-ani="fadeUp" data-delay="0.8" data-target=".ani-item-2" data-sequence="true">
            <div class="relative text-center">
                <div class="inline-block relative w-0 h-[64px] lg:h-[100px]">
                    <div class="absolute -left-[500px]">
                        <div class="w-[1000px] inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                            <div class="ani-item" data-classes="gradient-2"><?php the_custom_field('about_services.aboutservice_bgtitle'); ?></div>
                        </div>
                    </div>
                </div>
                <h2 class="ani-item-2 text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-white -mt-10"><?php the_custom_field('about_services.aboutservice_title'); ?></h2>
            </div>
        </div>
    </div>

    <div data-ani="fadeUp" data-delay="1.4" data-target=".ani-item" class="grid grid-cols-1 lg:grid-cols-3 gap-20 mt-20">
        <?php foreach(get_custom_field('about_services.aboutservice_items', []) as $idx => $item): ?>
        <div class="ani-item <?php echo ($idx % 2 == 0 ? '':'lg:pt-20'); ?>">
            <div class="h-[420px] lg:h-[600px] relative overflow-hidden rounded-[28px]">
                <img src="<?php the_value($item, 'aboutserviceitem_image'); ?>" class="absolute inset-0 w-full h-full pointer-events-none object-cover">
                <div class="absolute inset-0 w-full h-full pointer-events-none" style="background: linear-gradient(0deg, rgba(0, 0, 0, 0.5) 0%, rgba(0, 0, 0, 0) 100%);"></div>
                <div class="absolute z-10 left-0 bottom-0 w-full p-3">
                    <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'aboutserviceitem_title'); ?></h3>
                    <div class="my-3 border-t border-primary-300 opacity-20"></div>
                    <div class="text-[16px] lg:text-[18px] leading-175 text-gray-100"><?php the_value($item, 'aboutserviceitem_desc'); ?></div>
                </div>
            </div>
        </div>
        <?php endforeach; ?>
    </div>
</div>

<div class="max-w-[1280px] mx-auto px-3 lg:px-10 py-[100px]">
    <div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item" data-sequence="true">
        <div data-ani="fadeUp" data-delay="0.8" data-target=".ani-item-2" data-sequence="true">
            <div class="relative text-center">
                <div class="inline-block relative w-0 h-[64px] lg:h-[100px]">
                    <div class="absolute -left-[500px]">
                        <div class="w-[1000px] inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                            <div class="ani-item" data-classes="gradient-2"><?php the_custom_field('about_spirit.aboutspirit_bgtitle'); ?></div>
                        </div>
                    </div>
                </div>
                <h2 class="ani-item-2 text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-white -mt-10"><?php the_custom_field('about_spirit.aboutspirit_title'); ?></h2>
                <div class="ani-item-2 text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5"><?php the_custom_field('about_spirit.aboutspirit_desc'); ?></div>
            </div>
        </div>
    </div>

    <div data-ani="fadeUp" data-delay="1.4" data-target=".ani-item" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5 mt-20">
        <?php foreach(get_custom_field('about_spirit.aboutspirit_items', []) as $idx => $item): ?>
        <div class="card-1 p-10 text-center ani-item">
            <img src="<?php the_value($item, 'aboutspirititem_image'); ?>" class="w-[120px] mx-auto">
            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white mt-7"><?php the_value($item, 'aboutspirititem_title'); ?></h3>
            <p class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($item, 'aboutspirititem_desc'); ?></p>
        </div>
        <?php endforeach; ?>
    </div>
</div>

<div class="py-[100px]">
    <div class="h-[480px] relative overflow-hidden">
        <img data-parallax src="<?php the_custom_field('about_spirit.aboutspirit_image'); ?>" class="absolute inset-0 object-cover w-full h-full" style="opacity: 0;">
    </div>
</div>

<?php
get_template_part('template-parts/contactbanner');
get_footer();
