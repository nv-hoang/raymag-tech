<?php

/**
 * Template Name: Raymag Template - Home
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

<div data-ani="fadeUp" data-delay="2" data-target=".ani-item" data-sequence="true">
    <div data-ani="textfadeLeft2" data-delay="2.4" data-target=".ani-item-2" data-sequence="true">
        <div data-ani="fadeUp" data-delay="3" data-target=".ani-item-3" class="relative" data-sequence="true">
            <div class="absolute inset-0 pointer-events-none">
                <?php the_custom_field('home_kv.kv_backgroundscript'); ?>
            </div>

            <div class="relative z-10">
                <div class="max-w-[1440px] mx-auto px-3 lg:px-10 pt-[170px] lg:pt-[304px] pb-[170px] lg:pb-[224px]">
                    <div style="font-family: 'Inter';">
                        <?php
                        $title = explode('<br />', get_custom_field('home_kv.kv_title'), 2);
                        ?>
                        <div class="ani-item gradient-1 text-[40px] leading-125 font-semibold lg:text-[88px] lg:leading-100"><?php echo array_shift($title); ?></div>
                        <div class="ani-item-2 text-[40px] leading-125 font-semibold text-white lg:text-[88px] lg:leading-100"><?php echo array_shift($title); ?></div>
                    </div>
                    <div class="text-[20px] lg:text-[24px] leading-150 font-medium text-gray-100 mt-[100px] text-right">
                        <div class="ani-item-3">
                            <h3><?php the_custom_field('home_kv.kv_desc'); ?></h3>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="max-w-[1024px] mx-auto px-3 lg:px-10 text-center py-[100px]">
    <div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item" data-sequence="true">
        <div data-ani="fadeUp" data-delay="0.8" data-target=".ani-item-2" data-sequence="true">
            <div class="relative">
                <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                    <div class="ani-item" data-classes="gradient-2"><?php the_custom_field('home_products.pro_bgtitle'); ?></div>
                </div>
                <h1 class="ani-item-2 text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-white -mt-10"><?php the_custom_field('home_products.pro_title'); ?></h1>
            </div>
            <h6 class="ani-item-2 text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5"><?php the_custom_field('home_products.pro_desc'); ?></h6>

            <?php if (!empty(get_custom_field('home_products.pro_buttonlink.probtn_link'))): ?>
                <div class="mt-10 ani-item-2">
                    <a href="<?php the_custom_field('home_products.pro_buttonlink.probtn_link'); ?>" data-fancybox class="inline-block rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative">
                        <div class="inline-flex items-center gap-x-[10px] px-10 py-[10px] relative">
                            <div class="text-[16px] leading-175 font-medium text-white"><?php the_custom_field('home_products.pro_buttonlink.probtn_label'); ?></div>
                            <img src="<?php the_theme_asset_url('assets/img/icon-arrow.svg'); ?>">
                        </div>
                    </a>
                </div>
            <?php endif; ?>
        </div>
    </div>
</div>

<div class="max-w-[1920px] mx-auto px-3 lg:px-10 py-[100px]">
    <div data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
        <h2 class="ani-item text-center text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300"><?php the_custom_field('home_mpc.mpc_title'); ?></h2>
        <div class="grid grid-cols-1 lg:grid-cols-2 2xl:grid-cols-4 gap-5 mt-10">
            <?php
            $items = get_custom_field('home_mpc.mpc_items');
            ?>

            <?php if (isset($items[0])): ?>
                <div class="ani-item card-1 p-10 lg:py-[70px] 2xl:py-[56px] lg:grid-colspan-2">
                    <div class="flex flex-col gap-10 items-center lg:flex-row">
                        <img src="<?php the_value($items[0], 'mpcitem_image'); ?>" class="w-[120px] lg:w-[220px]">
                        <div class="flex-1">
                            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[0], 'mpcitem_title'); ?></h3>
                            <p class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[0], 'mpcitem_desc'); ?></p>
                        </div>
                    </div>
                </div>
            <?php endif; ?>

            <?php if (isset($items[1])): ?>
                <div class="ani-item card-1 p-10">
                    <div class="flex flex-col gap-10 lg:gap-7 items-center lg:items-start">
                        <img src="<?php the_value($items[1], 'mpcitem_image'); ?>" class="w-[120px]">
                        <div class="flex-1">
                            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[1], 'mpcitem_title'); ?></h3>
                            <p class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[1], 'mpcitem_desc'); ?></p>
                        </div>
                    </div>
                </div>
            <?php endif; ?>

            <?php if (isset($items[2])): ?>
                <div class="ani-item card-1 p-10">
                    <div class="flex flex-col gap-10 lg:gap-7 items-center lg:items-start">
                        <img src="<?php the_value($items[2], 'mpcitem_image'); ?>" class="w-[120px]">
                        <div class="flex-1">
                            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[2], 'mpcitem_title'); ?></h3>
                            <p class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[2], 'mpcitem_desc'); ?></p>
                        </div>
                    </div>
                </div>
            <?php endif; ?>

            <?php if (isset($items[3])): ?>
                <div class="ani-item card-1 p-10">
                    <div class="flex flex-col gap-10 lg:gap-7 items-center lg:items-start">
                        <img src="<?php the_value($items[3], 'mpcitem_image'); ?>" class="w-[120px]">
                        <div class="flex-1">
                            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[3], 'mpcitem_title'); ?></h3>
                            <p class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[3], 'mpcitem_desc'); ?></p>
                        </div>
                    </div>
                </div>
            <?php endif; ?>

            <?php if (isset($items[4])): ?>
                <div class="ani-item card-1 p-10">
                    <div class="flex flex-col gap-10 lg:gap-7 items-center lg:items-start">
                        <img src="<?php the_value($items[4], 'mpcitem_image'); ?>" class="w-[120px]">
                        <div class="flex-1">
                            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[4], 'mpcitem_title'); ?></h3>
                            <p class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[4], 'mpcitem_desc'); ?></p>
                        </div>
                    </div>
                </div>
            <?php endif; ?>

            <?php if (isset($items[5])): ?>
                <div class="ani-item card-1 p-10 lg:py-[66px] 2xl:py-[56px] lg:grid-colspan-2">
                    <div class="flex flex-col gap-10 items-center lg:flex-row">
                        <img src="<?php the_value($items[5], 'mpcitem_image'); ?>" class="w-[120px] lg:w-[220px]">
                        <div class="flex-1">
                            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[5], 'mpcitem_title'); ?></h3>
                            <p class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[5], 'mpcitem_desc'); ?></p>
                        </div>
                    </div>
                </div>
            <?php endif; ?>
        </div>
    </div>
</div>

<div class="max-w-[1440px] mx-auto py-[100px] px-3 lg:px-10">
    <div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item" data-sequence="true">
        <div data-ani="fadeUp" data-delay="0.8" data-target=".ani-item-2" data-sequence="true">
            <div class="relative">
                <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                    <div class="ani-item" data-classes="gradient-2"><?php the_custom_field('home_service.ser_bgtitle'); ?></div>
                </div>
                <h1 class="ani-item-2 text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-white -mt-10"><?php the_custom_field('home_service.ser_title'); ?></h1>
            </div>
            <h6 class="ani-item-2 text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5"><?php the_custom_field('home_service.ser_desc'); ?></h6>
        </div>
    </div>

    <div class="relative mt-[80px]">
        <div x-init="$dispatch('initswiper')" id="service-swiper" data-effect-speed="600" data-loop="true" data-effect="fade" data-next=".service-swiper-next" data-prev=".service-swiper-prev" data-index=".service-swiper-index" data-total=".service-swiper-total" class="relative swiper-ani">
            <div> <!-- swiper-wrapper -->
                <?php foreach (get_custom_field('home_service.ser_slider') as $idx => $item): ?>
                    <div class="swiper-slide swiper-slide-<?php echo $idx; ?>">
                        <div data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
                            <div class="relative">
                                <div class="lg:flex lg:min-h-[720px] gap-[80px] relative">
                                    <div class="hidden lg:block w-[580px]"></div>
                                    <div class="flex-1">
                                        <div class="lg:pt-10 lg:min-h-[300px] lg:max-w-[520px] lg:mx-auto">
                                            <div class="text-[40px] lg:text-[64px] leading-125 lg:leading-100 font-semibold overflow-hidden" style="font-family: 'Inter';">
                                                <div class="ani-item gradient-1"><?php the_value($item, 'seritem_title'); ?></div>
                                            </div>
                                            <h2 class="text-[24px] lg:text-[32px] leading-150 font-medium text-white mt-5 overflow-hidden">
                                                <div class="ani-item"><?php the_value($item, 'seritem_subtitle'); ?></div>
                                            </h2>
                                        </div>

                                        <div class="grid grid-cols-1 lg:grid-cols-2 gap-10 lg:gap-[80px] mt-10">
                                            <div class="grid lg:block grid-cols-2 gap-3 lg:order-1">
                                                <div class="lg:absolute lg:w-[580px] lg:max-w-[50%] lg:h-[720px] lg:top-0 lg:left-0">
                                                    <div data-ani="boxX" data-delay="0.4">
                                                        <div class="inline-flex rounded-[20px] overflow-hidden">
                                                            <img src="<?php the_value($item, 'seritem_image1'); ?>" class="w-full h-full object-cover">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="pt-10 lg:pt-0">
                                                    <div data-ani="boxX" data-delay="0.4">
                                                        <div class="inline-flex rounded-[20px] overflow-hidden">
                                                            <img src="<?php the_value($item, 'seritem_image2'); ?>" class="w-full h-full object-cover">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <h6 class="text-[16px] lg:text-[18px] leading-175 text-gray-100 overflow-hidden">
                                                    <div class="ani-item"><?php the_value($item, 'seritem_desc'); ?></div>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                <?php endforeach; ?>
            </div>
        </div>

        <div class="mt-10 lg:mt-0 lg:absolute z-10 lg:left-[660px] lg:bottom-10">
            <div class="flex items-center justify-center gap-5 lg:justify-start">
                <div class="service-swiper-prev inline-block rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative">
                    <div class="size-[48px] flex items-center justify-center">
                        <img src="<?php the_theme_asset_url('assets/img/icon-arrow.svg'); ?>" class="relative rotate-180">
                    </div>
                </div>

                <div class="flex items-center gap-2 text-[16px] leading-175 text-gray-100">
                    <div class="service-swiper-index"></div>
                    <div>/</div>
                    <div class="service-swiper-total"></div>
                </div>

                <div class="service-swiper-next inline-block rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative">
                    <div class="size-[48px] flex items-center justify-center">
                        <img src="<?php the_theme_asset_url('assets/img/icon-arrow.svg'); ?>" class="relative">
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="max-w-[1920px] mx-auto py-[100px] px-3 lg:px-10">
    <div data-ani="fadeUp" data-delay="1.2" data-target=".ani-item" class="grid grid-cols-1 lg:grid-cols-2 2xl:grid-cols-4 gap-5">
        <div class="lg:grid-colspan-2 pb-[60px] 2xl:p-10">
            <div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item-2" data-sequence="true">
                <div data-ani="fadeUp" data-delay="0.8" data-target=".ani-item-3" data-sequence="true">
                    <div class="relative">
                        <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                            <div class="ani-item-2" data-classes="gradient-2"><?php the_custom_field('home_equipment.equ_bgtitle'); ?></div>
                        </div>
                        <h1 class="ani-item-3 text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-white -mt-10"><?php the_custom_field('home_equipment.equ_title'); ?></h1>
                    </div>
                    <h6 class="ani-item-3 text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5"><?php the_custom_field('home_equipment.equ_desc'); ?></h6>
                </div>
            </div>
        </div>

        <?php
        $items = get_custom_field('home_equipment.equ_items');
        ?>

        <?php if (isset($items[0])): ?>
            <div class="lg:grid-colspan-2">
                <div class="ani-item relative h-[445px] overflow-hidden rounded-[28px]">
                    <img src="<?php the_value($items[0], 'equitem_image'); ?>" class="absolute inset-0 w-full h-full object-cover">
                    <div class="absolute inset-0 w-full h-full bg-black bg-opacity-20"></div>
                    <div class="absolute bottom-0 left-0 w-full z-10 p-5">
                        <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[0], 'equitem_title'); ?></h3>
                        <div class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[0], 'equitem_desc'); ?></div>
                    </div>
                </div>
            </div>
        <?php endif; ?>

        <?php if (isset($items[1])): ?>
            <div class="lg:grid-colspan-2">
                <div class="ani-item relative h-[445px] overflow-hidden rounded-[28px]">
                    <img src="<?php the_value($items[1], 'equitem_image'); ?>" class="absolute inset-0 w-full h-full object-cover">
                    <div class="absolute inset-0 w-full h-full bg-black bg-opacity-20"></div>
                    <div class="absolute bottom-0 left-0 w-full z-10 p-5">
                        <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[1], 'equitem_title'); ?></h3>
                        <div class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[1], 'equitem_desc'); ?></div>
                    </div>
                </div>
            </div>
        <?php endif; ?>

        <?php if (isset($items[2])): ?>
            <div>
                <div class="ani-item relative h-[445px] overflow-hidden rounded-[28px]">
                    <img src="<?php the_value($items[2], 'equitem_image'); ?>" class="absolute inset-0 w-full h-full object-cover">
                    <div class="absolute inset-0 w-full h-full bg-black bg-opacity-20"></div>
                    <div class="absolute bottom-0 left-0 w-full z-10 p-5">
                        <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[2], 'equitem_title'); ?></h3>
                        <div class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[2], 'equitem_desc'); ?></div>
                    </div>
                </div>
            </div>
        <?php endif; ?>

        <?php if (isset($items[3])): ?>
            <div>
                <div class="ani-item relative h-[445px] overflow-hidden rounded-[28px]">
                    <img src="<?php the_value($items[3], 'equitem_image'); ?>" class="absolute inset-0 w-full h-full object-cover">
                    <div class="absolute inset-0 w-full h-full bg-black bg-opacity-20"></div>
                    <div class="absolute bottom-0 left-0 w-full z-10 p-5">
                        <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($items[3], 'equitem_title'); ?></h3>
                        <div class="text-[16px] leading-175 text-gray-100 mt-3"><?php the_value($items[3], 'equitem_desc'); ?></div>
                    </div>
                </div>
            </div>
        <?php endif; ?>
    </div>
</div>

<div class="relative overflow-hidden">
    <img src="<?php the_theme_asset_url('assets/img/bg-home-contact.webp'); ?>" class="absolute inset-0 w-full h-full object-cover pointer-events-none">
    <div class="relative z-10">
        <?php if (false): ?>
            <div class="max-w-[1024px] mx-auto py-[100px] px-3 lg:px-10">
                <div class="text-center">
                    <div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item" data-sequence="true">
                        <div data-ani="fadeUp" data-delay="0.8" data-target=".ani-item-2" data-sequence="true">
                            <div class="relative">
                                <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                                    <div class="ani-item" data-classes="gradient-2">NEWS</div>
                                </div>
                                <h1 class="ani-item-2 text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-white -mt-10">最新消息</h1>
                            </div>
                        </div>
                    </div>
                </div>

                <div data-ani="fadeUp" data-delay="1" data-target=".ani-item" class="mt-[80px]">
                    <div class="ani-item">
                        <a href="#" class="flex flex-col lg:flex-row gap-3 lg:gap-10 hover-scale">
                            <div class="lg:w-[320px]">
                                <div class="inline-block w-full h-[210px] lg:h-[200px] overflow-hidden rounded-[8px]">
                                    <img src="<?php the_theme_asset_url('assets/img/pic-home-news-1.webp'); ?>" class="w-full h-full object-cover transition-transform">
                                </div>
                            </div>
                            <div class="flex-1 px-3 lg:p-3">
                                <div class="text-[16px] leading-175 text-primary-300 text-right">January 1, 2025</div>
                                <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white mt-3">全方位金屬表面處理技術</h3>
                                <p class="text-[16px] leading-175 text-gray-100 mt-3">透過創新的製程與專業技術，提供金屬材料耐蝕性、強度與外觀品質的全方位解決方案，滿足多元產業的應用需求。</p>
                            </div>
                        </a>
                    </div>

                    <div class="ani-item mt-7 pt-7 border-t border-primary-300 border-opacity-20">
                        <a href="#" class="flex flex-col lg:flex-row gap-3 lg:gap-10 hover-scale">
                            <div class="lg:w-[320px]">
                                <div class="inline-block w-full h-[210px] lg:h-[200px] overflow-hidden rounded-[8px]">
                                    <img src="<?php the_theme_asset_url('assets/img/pic-home-news-2.webp'); ?>" class="w-full h-full object-cover transition-transform">
                                </div>
                            </div>
                            <div class="flex-1 px-3 lg:p-3">
                                <div class="text-[16px] leading-175 text-primary-300 text-right">January 1, 2025</div>
                                <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white mt-3">全方位金屬表面處理技術</h3>
                                <p class="text-[16px] leading-175 text-gray-100 mt-3">透過創新的製程與專業技術，提供金屬材料耐蝕性、強度與外觀品質的全方位解決方案，滿足多元產業的應用需求。</p>
                            </div>
                        </a>
                    </div>

                    <div class="ani-item mt-7 pt-7 border-t border-primary-300 border-opacity-20">
                        <a href="#" class="flex flex-col lg:flex-row gap-3 lg:gap-10 hover-scale">
                            <div class="lg:w-[320px]">
                                <div class="inline-block w-full h-[210px] lg:h-[200px] overflow-hidden rounded-[8px]">
                                    <img src="<?php the_theme_asset_url('assets/img/pic-home-news-3.webp'); ?>" class="w-full h-full object-cover transition-transform">
                                </div>
                            </div>
                            <div class="flex-1 px-3 lg:p-3">
                                <div class="text-[16px] leading-175 text-primary-300 text-right">January 1, 2025</div>
                                <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white mt-3">全方位金屬表面處理技術</h3>
                                <p class="text-[16px] leading-175 text-gray-100 mt-3">透過創新的製程與專業技術，提供金屬材料耐蝕性、強度與外觀品質的全方位解決方案，滿足多元產業的應用需求。</p>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
        <?php endif; ?>

        <div id="section-contact" class="max-w-[1440px] mx-auto py-[100px]">
            <div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item" data-sequence="true">
                <div data-ani="fadeUp" data-delay="0.8" data-target=".ani-item-2" data-sequence="true">
                    <div class="grid grid-cols-1 lg:grid-cols-2 gap-[80px]">
                        <div class="px-3 lg:px-10">
                            <div>
                                <div class="relative">
                                    <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                                        <div class="ani-item" data-classes="gradient-2"><?php the_custom_field('home_contact.contact_bgtitle'); ?></div>
                                    </div>
                                    <h1 class="ani-item-2 text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-white -mt-10"><?php the_custom_field('home_contact.contact_title'); ?></h1>
                                </div>
                                <h6 class="ani-item-2 text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5"><?php the_custom_field('home_contact.contact_desc'); ?></h6>
                            </div>

                            <div class="ani-item-2 mt-[60px]">
                                <img src="<?php the_theme_option('general.logo'); ?>" class="h-[48px]">
                                <h5 class="text-[16px] lg:text-[18px] leading-175 font-medium text-white mt-5"><?php the_theme_option('contactinfo.contact', true); ?></h5>

                                <div class="flex flex-col gap-3 mt-5">
                                    <?php if (!empty(get_theme_option('contactinfo.phone'))): ?>
                                        <div class="flex items-start gap-[10px]">
                                            <img src="<?php the_theme_asset_url('assets/img/icon-contact-2.svg'); ?>">
                                            <a href="tel:<?php the_theme_option('contactinfo.phone'); ?>" class="text-[16px] leading-175 text-gray-100 hover:text-primary-300 transition-colors"><?php the_theme_option('contactinfo.phone'); ?></a>
                                        </div>
                                    <?php endif; ?>

                                    <?php if (!empty(get_theme_option('contactinfo.fax'))): ?>
                                        <div class="flex items-start gap-[10px]">
                                            <img src="<?php the_theme_asset_url('assets/img/icon-contact-3.svg'); ?>">
                                            <a href="fax:<?php the_theme_option('contactinfo.fax'); ?>" class="text-[16px] leading-175 text-gray-100 hover:text-primary-300 transition-colors"><?php the_theme_option('contactinfo.fax'); ?></a>
                                        </div>
                                    <?php endif; ?>

                                    <?php if (!empty(get_theme_option('contactinfo.address', true))): ?>
                                        <div class="flex items-start gap-[10px]">
                                            <img src="<?php the_theme_asset_url('assets/img/icon-contact-1.svg'); ?>">
                                            <a href="https://www.google.com/maps?q=<?php the_theme_option('contactinfo.address', true); ?>" target="_blank" class="text-[16px] leading-175 text-gray-100 hover:text-primary-300 transition-colors"><?php the_theme_option('contactinfo.address', true); ?></a>
                                        </div>
                                    <?php endif; ?>

                                    <?php if (!empty(get_theme_option('contactinfo.email'))): ?>
                                        <div class="flex items-start gap-[10px]">
                                            <img src="<?php the_theme_asset_url('assets/img/icon-contact-4.svg'); ?>">
                                            <a href="mailto:<?php the_theme_option('contactinfo.email'); ?>" class="text-[16px] leading-175 text-gray-100 hover:text-primary-300 transition-colors"><?php the_theme_option('contactinfo.email'); ?></a>
                                        </div>
                                    <?php endif; ?>
                                </div>
                            </div>
                        </div>

                        <div class="px-3 lg:px-10">
                            <div class="ani-item-2">
                                <?php if ($form = get_custom_field('home_contact.contact_form')): ?>
                                    <p class="text-right text-[16px] leading-175 text-primary-300" style="margin-bottom: 14px;">*為必填欄位</p>
                                    <?php echo do_shortcode('[wpforms id="' . $form->ID . '"]'); ?>
                                <?php endif; ?>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
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
