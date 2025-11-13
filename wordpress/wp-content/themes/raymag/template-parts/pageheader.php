<?php
/**
 * Template part
 *
 * @link https://developer.wordpress.org/themes/basics/template-hierarchy/
 *
 * @package raymag
 */
?>

<div class="relative" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item" data-sequence="true">
    <img src="<?php the_custom_field('pageheader.pageheader_image'); ?>" class="absolute inset-0 w-full h-full object-cover">
    <div class="absolute inset-0 w-full h-full" style="background: linear-gradient(180deg, rgba(3, 10, 17, 0.5) 0%, #030A11 100%);"></div>
    <div class="relative z-10 max-w-[768px] mx-auto px-3 lg:px-10 text-center py-[100px] lg:py-[200px] lg:min-h-[630px]">
        <h1 class="ani-item text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-primary-300"><?php the_custom_field('pageheader.pageheader_title'); ?></h1>
        <div class="ani-item mt-5 flex flex-col gap-1">
            <?php if($subtitle = get_custom_field('pageheader.pageheader_subtitle')): ?>
                <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php echo $subtitle; ?></h3>
            <?php endif; ?>

            <?php if($desc = get_custom_field('pageheader.pageheader_desc')): ?>
                <h6 class="text-[16px] lg:text-[18px] leading-175 text-gray-100"><?php echo $desc; ?></h6>
            <?php endif; ?>
        </div>

        <?php if($link = get_custom_field('pageheader.pageheader_button.pageheaderbtn_link')): ?>
            <div class="ani-item mt-10 text-center">
                <a href="<?php echo $link; ?>" class="inline-block rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative">
                    <div class="inline-flex items-center gap-x-[10px] px-10 py-[10px] relative">
                        <div class="text-[16px] leading-175 font-medium text-white"><?php the_custom_field('pageheader.pageheader_button.pageheaderbtn_label'); ?></div>
                        <img src="<?php the_theme_asset_url('assets/img/icon-arrow.svg'); ?>">
                    </div>
                </a>
            </div>
        <?php endif; ?>
    </div>
</div>