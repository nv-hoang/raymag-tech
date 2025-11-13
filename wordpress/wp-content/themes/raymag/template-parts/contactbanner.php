<?php

/**
 * Template part
 *
 * @link https://developer.wordpress.org/themes/basics/template-hierarchy/
 *
 * @package raymag
 */
?>
<div class="relative overflow-hidden">
    <img src="<?php the_theme_asset_url('assets/img/bg-post-contact.png'); ?>" class="absolute inset-0 w-full h-full object-cover pointer-events-none">
    <div class="absolute inset-0 w-full h-full pointer-events-none" style="background: linear-gradient(180deg, #030A11 25%, rgba(3, 10, 17, 0.5) 75%);"></div>
    <div class="relative z-10 max-w-[1024px] mx-auto px-3 lg:px-10 pt-[100px] pb-[200px]">
        <div data-ani="textfadeLeft2" data-delay="0.4" data-target=".ani-item" data-sequence="true">
            <div data-ani="fadeUp" data-delay="0.8" data-target=".ani-item-2" data-sequence="true">
                <div class="relative text-center">
                    <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                        <div class="ani-item" data-classes="gradient-2"><?php the_theme_option('contactbanner.contactbanner_bgtitle'); ?></div>
                    </div>
                    <h1 class="ani-item-2 text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-white -mt-10"><?php the_theme_option('contactbanner.contactbanner_title', true); ?></h1>
                </div>

                <div class="ani-item-2 text-center mt-5 text-[16px] lg:text-[18px] leading-175 text-gray-100">
                    <?php the_theme_option('contactbanner.contactbanner_desc', true); ?>
                </div>

                <div class="ani-item-2 mt-10 flex items-center justify-center flex-col sm:flex-row gap-5">
                    <div>
                        <a href="<?php echo get_permalink(get_theme_option('contactbanner.contactbanner_btn1.contactbannerbtn1_link')); ?>" class="inline-block rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative">
                            <div class="inline-flex items-center gap-x-[10px] px-10 py-[10px] relative">
                                <div class="text-[16px] leading-175 font-medium text-white"><?php the_theme_option('contactbanner.contactbanner_btn1.contactbannerbtn1_label', true); ?></div>
                                <img src="<?php the_theme_asset_url('assets/img/icon-arrow.svg'); ?>">
                            </div>
                        </a>
                    </div>

                    <div>
                        <a href="<?php the_theme_option('contactbanner.contactbanner_btn2.contactbannerbtn2_link'); ?>" class="inline-block rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative">
                            <div class="inline-flex items-center gap-x-[10px] px-10 py-[10px] relative">
                                <div class="text-[16px] leading-175 font-medium text-white"><?php the_theme_option('contactbanner.contactbanner_btn2.contactbannerbtn2_label', true); ?></div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>