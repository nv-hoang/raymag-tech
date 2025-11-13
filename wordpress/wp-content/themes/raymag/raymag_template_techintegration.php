<?php

/**
 * Template Name: Raymag Template - Tech-integration (科技工程整合方案)
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
get_template_part('template-parts/pageheader');
?>

<div class="max-w-[1440px] mx-auto px-3 lg:px-10 py-[100px]">
    <h2 data-ani="fadeUp" data-delay="0.4" class="text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center"><?php the_custom_field('techintegration_core.techintegrationcore_title'); ?></h2>

    <div class="flex flex-col gap-[100px] mt-10">
        <?php foreach(get_custom_field('techintegration_core.techintegrationcore_items', []) as $idx => $item): ?>
            <div class="flex flex-col lg:flex-row gap-10 xl:gap-20" data-ani="fadeUp" data-delay="0.8" data-target=".ani-item" data-sequence="true">
                <div class="ani-item flex-1 flex flex-col justify-center <?php echo ($idx % 2 == 0 ? 'lg:order-1':''); ?>">
                    <div class="relative xl:pb-20">
                        <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                            <div class="gradient-2"><?php echo str_pad($idx+1, 2, '0', STR_PAD_LEFT); ?></div>
                        </div>
                        <div class="-mt-5 pl-10">
                            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'techintegrationcoreitem_title'); ?></h3>
                            <div class="text-[16px] leading-175 text-gray-100 mt-3 editor-content-style" style="--li-mb: 8px;">
                                <?php the_value($item, 'techintegrationcoreitem_content'); ?>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="ani-item lg:w-[780px] lg:max-w-[60%]">
                    <div x-init="$dispatch('initswiper')" id="core-swiper-0" data-autoplay="5000" data-loop="true" class="relative swiper-ani">
                        <div> <!-- swiper-wrapper -->
                            <?php foreach(get_value($item, 'techintegrationcoreitem_images', []) as $idx2=>$slide): ?>
                                <div class="swiper-slide">
                                    <img src="<?php the_value($slide, 'techintegrationcoreitemimage_img'); ?>" class="w-full rounded-[28px]">
                                    <div class="text-[16px] leading-175 font-medium text-primary-300 mt-3 px-[10px]"><?php the_value($slide, 'techintegrationcoreitemimage_desc'); ?></div>
                                </div>
                            <?php endforeach; ?>
                        </div>

                        <div class="swiper-prev absolute left-3 top-[calc(50%-56px)] md:top-[calc(50%-40px)] z-10">
                            <div class="opacity-80 hover:opacity-100 size-[48px] rounded-full border border-primary-300 border-opacity-20 flex items-center justify-center" style="background: #030A11E5;">
                                <svg width="18" height="18" viewBox="0 0 18 18" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M16.594 8.99995C16.594 8.77617 16.5051 8.56156 16.3469 8.40333C16.1886 8.24509 15.974 8.1562 15.7502 8.1562L4.28648 8.1562L7.34648 5.0962C7.49552 4.93625 7.57666 4.7247 7.57281 4.50611C7.56895 4.28752 7.4804 4.07897 7.32581 3.92438C7.17122 3.76979 6.96266 3.68123 6.74407 3.67738C6.52548 3.67352 6.31393 3.75466 6.15398 3.9037L1.65398 8.4037C1.49598 8.5619 1.40723 8.77636 1.40723 8.99995C1.40723 9.22354 1.49598 9.438 1.65398 9.5962L6.15398 14.0962C6.31393 14.2452 6.52548 14.3264 6.74407 14.3225C6.96266 14.3187 7.17122 14.2301 7.32581 14.0755C7.4804 13.9209 7.56895 13.7124 7.57281 13.4938C7.57666 13.2752 7.49552 13.0636 7.34648 12.9037L4.28648 9.8437L15.7502 9.8437C15.974 9.8437 16.1886 9.7548 16.3469 9.59657C16.5051 9.43834 16.594 9.22373 16.594 8.99995Z" fill="#A0CDE4" />
                                </svg>
                            </div>
                        </div>

                        <div class="swiper-next absolute right-3 top-[calc(50%-56px)] md:top-[calc(50%-40px)] z-10">
                            <div class="opacity-80 hover:opacity-100 size-[48px] rounded-full border border-primary-300 border-opacity-20 flex items-center justify-center" style="background: #030A11E5;">
                                <svg width="18" height="18" viewBox="0 0 18 18" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M1.40602 8.99995C1.40602 8.77617 1.49491 8.56156 1.65315 8.40333C1.81138 8.24509 2.02599 8.1562 2.24977 8.1562L13.7135 8.1562L10.6535 5.0962C10.5045 4.93625 10.4233 4.7247 10.4272 4.50611C10.4311 4.28752 10.5196 4.07897 10.6742 3.92438C10.8288 3.76979 11.0373 3.68123 11.2559 3.67738C11.4745 3.67352 11.6861 3.75466 11.846 3.9037L16.346 8.4037C16.504 8.5619 16.5928 8.77636 16.5928 8.99995C16.5928 9.22354 16.504 9.438 16.346 9.5962L11.846 14.0962C11.6861 14.2452 11.4745 14.3264 11.2559 14.3225C11.0373 14.3187 10.8288 14.2301 10.6742 14.0755C10.5196 13.9209 10.4311 13.7124 10.4272 13.4938C10.4233 13.2752 10.5045 13.0636 10.6535 12.9037L13.7135 9.8437L2.24977 9.8437C2.02599 9.8437 1.81138 9.7548 1.65315 9.59657C1.49491 9.43834 1.40602 9.22373 1.40602 8.99995Z" fill="#A0CDE4" />
                                </svg>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <?php endforeach; ?>
    </div>
</div>

<div class="max-w-[1680px] mx-auto px-3 lg:px-10 py-[100px]" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <h2 class="ani-item text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center"><?php the_custom_field('techintegration_solutions.techintegrationsolution_title'); ?></h2>

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-[60px] mt-10">
        <?php foreach(get_custom_field('techintegration_solutions.techintegrationsolution_items', []) as $idx => $item): ?>
            <div class="ani-item">
                <img src="<?php the_value($item, 'techintegrationsolutionitem_image'); ?>" class="w-full rounded-[28px]">
                <div class="px-3 mt-5">
                    <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'techintegrationsolutionitem_title'); ?></h3>
                    <div class="border-t border-primary-300 opacity-20 my-3"></div>
                    <div class="text-[18px] leading-175 text-gray-100"><?php the_value($item, 'techintegrationsolutionitem_desc'); ?></div>
                </div>
            </div>
        <?php endforeach; ?>
    </div>
</div>

<div class="py-[100px]">
    <div class="relative max-w-[1440px] mx-auto lg:py-[150px] 2xl:py-[172px]">
        <div class="relative z-10 lg:max-w-[680px] lg:ml-auto px-3 lg:px-10 pb-[80px]" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item" data-sequence="true">
            <h2 class="ani-item text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300"><?php the_custom_field('techintegration_whyus.techintegrationwhyus_title'); ?></h2>
            <h6 class="ani-item text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5"><?php the_custom_field('techintegration_whyus.techintegrationwhyus_desc'); ?></h6>
            <div class="mt-10 flex flex-col gap-5">
                <?php foreach(get_custom_field('techintegration_whyus.techintegrationwhyus_items', []) as $idx => $item): ?>
                    <div class="ani-item flex items-start gap-5">
                        <div><img src="<?php the_theme_asset_url('assets/img/icon-tech-integration-why-us.svg'); ?>" class="w-[30px] md:w-[36px]"></div>
                        <div class="flex-1 text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'techintegrationwhyusitem_title'); ?></div>
                    </div>
                <?php endforeach; ?>
            </div>
        </div>
        <img src="<?php the_custom_field('techintegration_whyus.techintegrationwhyus_image'); ?>" class="w-full lg:absolute lg:top-0 lg:-left-[240px]">
    </div>
</div>

<div class="max-w-[1280px] mx-auto px-3 lg:px-10 py-[100px]" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <h2 class="text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center ani-item"><?php the_custom_field('techintegration_success.techintegrationsuccess_title'); ?></h2>

    <div class="flex flex-col gap-10 mt-10">
        <?php foreach(get_custom_field('techintegration_success.techintegrationsuccess_items', []) as $idx => $item): ?>
            <div class="card-1 p-5 ani-item">
                <div class="flex flex-col lg:flex-row gap-5 lg:gap-10">
                    <div class="lg:w-[400px]">
                        <img src="<?php the_value($item, 'techintegrationsuccessitem_image'); ?>" class="w-full rounded-[14px]">
                    </div>
                    <div class="flex-1 flex flex-col justify-center">
                        <div class="px-3">
                            <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'techintegrationsuccessitem_title'); ?></h3>
                            <div class="border-t border-primary-300 opacity-20 my-3"></div>
                            <div class="text-[16px] lg:text-[18px] leading-175 text-gray-100">
                                <?php 
                                    $techintegrationsuccessitem_desc = get_value($item, 'techintegrationsuccessitem_desc');
                                    if (preg_match('/\[\s*([^\]]+)\s*\]/u', $techintegrationsuccessitem_desc, $m)) {
                                        $techintegrationsuccessitem_desc = str_replace($m[0], '<span class="inline-block mr-[10px] text-primary-500 font-semibold text-[16px] lg:text-[18px]">'.$m[1].'</span>', $techintegrationsuccessitem_desc);
                                    }

                                    if (preg_match('/\{\s*(\d+)\s*\}/u', $techintegrationsuccessitem_desc, $m)) {
                                        // echo "full: " . $m[0] . PHP_EOL;   // outputs: {30}
                                        // echo "number: " . $m[1] . PHP_EOL; // outputs: 30
                                        $techintegrationsuccessitem_desc = str_replace($m[0], '<span class="inline-block mx-[10px] text-primary-500 font-semibold text-[40px] lg:text-[64px] leading-125 lg:leading-100" data-sub-ani="countUp" data-num="'.$m[1].'">&nbsp;</span>', $techintegrationsuccessitem_desc);
                                    }
                                    
                                    echo $techintegrationsuccessitem_desc;
                                ?>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <?php endforeach; ?>
    </div>
</div>
<?php
get_template_part('template-parts/contactbanner');
get_footer();
