<?php

/**
 * Template Name: Raymag Template - Surface-equipment (表面處理設備)
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
    <h2 data-ani="fadeUp" data-delay="0.4" class="text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center"><?php the_custom_field('surfaceequipment_process.surfaceequipmentprocess_title'); ?></h2>

    <div class="relative pl-[60px] md:pl-0 mt-10 timeline">
        <div class="absolute left-[23px] md:left-[50%] h-full border-l border-[#283943]"></div>
        <div class="absolute left-[23px] md:left-[50%] border-l border-primary-500 timeline-progress"></div>

        <div class="flex flex-col gap-20">
            <?php 
                $items = get_custom_field('surfaceequipment_process.surfaceequipmentprocess_items', []);
                $cnt = count($items) - 1;
            ?>
            <?php foreach($items as $idx => $item): ?>
                <div class="relative timeline-item">
                    <?php if($idx == 0): ?>
                        <div class="absolute -left-[37px] md:left-[50%] h-[83px] md:h-[50%] border-l border-[#030A11]"></div>
                    <?php endif; ?>
                    <?php if($idx == $cnt): ?>
                        <div class="absolute bottom-0 -left-[37px] md:left-[50%] h-[calc(100%-83px)] md:h-[50%] border-l border-[#030A11]"></div>
                    <?php endif; ?>

                    <div class="absolute top-[83px] md:top-[50%] -left-[37px] md:left-[50%] w-0 h-0">
                        <div class="absolute -left-[3px] -top-[3px] size-2 bg-primary-500 rounded-full timeline-item-point"></div>
                        <div class="absolute -left-[23px] -top-[23px] size-[48px] border border-[#283943] rounded-full"></div>
                    </div>

                    <div class="grid grid-cols-1 md:grid-cols-2 gap-3 md:gap-[128px]">
                        <div class="<?php echo ($idx % 2 == 0 ? '':'md:order-1'); ?>">
                            <div class="timeline-item-img" style="opacity: 0;">
                                <div class="inline-flex rounded-[28px] overflow-hidden">
                                    <img src="<?php the_value($item, 'surfaceequipmentprocessitem_image'); ?>" class="w-full h-full object-cover">
                                </div>
                            </div>
                        </div>
                        <div class="flex flex-col justify-center">
                            <div class="relative xl:pb-20 timeline-item-info" style="opacity: 0;">
                                <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                                    <div class="gradient-2"><?php echo str_pad($idx+1, 2, '0', STR_PAD_LEFT); ?></div>
                                </div>
                                <div class="-mt-5 pl-10">
                                    <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'surfaceequipmentprocessitem_title'); ?></h3>
                                    <div class="text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-3 editor-content-style" style="--li-mb: 8px;">
                                        <?php the_value($item, 'surfaceequipmentprocessitem_content'); ?>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            <?php endforeach; ?>

        </div>
    </div>
</div>

<div class="max-w-[1680px] mx-auto px-3 lg:px-10 py-[100px]" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <h2 class="text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center ani-item"><?php the_custom_field('surfaceequipment_solutions.surfaceequipmentsolution_title'); ?></h2>

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-[60px] lg:gap-y-[80px] mt-10">
        <?php foreach(get_custom_field('surfaceequipment_solutions.surfaceequipmentsolution_items', []) as $idx => $item): ?>
            <div class="ani-item">
                <img src="<?php the_value($item, 'surfaceequipmentsolutionitem_image'); ?>" class="w-full rounded-[28px]">
                <div class="px-3 mt-5">
                    <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'surfaceequipmentsolutionitem_title'); ?></h3>
                    <div class="border-t border-primary-300 opacity-20 my-3"></div>
                    <div class="text-[18px] leading-175 text-gray-100  editor-content-style" style="--li-mb: 8px;">
                        <?php the_value($item, 'surfaceequipmentsolutionitem_content'); ?>
                    </div>
                </div>
            </div>
        <?php endforeach; ?>
    </div>
</div>

<div class="max-w-[1680px] mx-auto px-3 lg:px-10 py-[100px]" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <h2 class="text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center ani-item"><?php the_custom_field('surfaceequipment_integration.surfaceequipmentintegration_title'); ?></h2>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-20 lg:gap-y-10 mt-10">
        <?php foreach(get_custom_field('surfaceequipment_integration.surfaceequipmentintegration_items', []) as $idx => $item): ?>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-3 md:gap-10 ani-item">
                <div>
                    <img src="<?php the_value($item, 'surfaceequipmentintegrationitem_image'); ?>" class="rounded-[28px] w-full">
                </div>
                <div class="flex flex-col justify-center">
                    <div>
                        <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'surfaceequipmentintegrationitem_title'); ?></h3>
                        <div class="border-t border-primary-300 opacity-20 my-3"></div>
                        <div class="text-[16px] lg:text-[18px] leading-175 text-gray-100"><?php the_value($item, 'surfaceequipmentintegrationitem_desc'); ?></div>
                    </div>
                </div>
            </div>
        <?php endforeach; ?>

    </div>
</div>

<div class="px-3 lg:px-10 py-[100px]" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <h2 class="text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center ani-item"><?php the_custom_field('surfaceequipment_services.surfaceequipmentservice_title'); ?></h2>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 2xl:grid-cols-5 gap-5 mt-10">
        <?php foreach(get_custom_field('surfaceequipment_services.surfaceequipmentservice_items', []) as $idx => $item): ?>
            <div class="card-1 p-10 text-center ani-item ani-item">
                <img src="<?php the_value($item, 'surfaceequipmentserviceitem_image'); ?>" class="w-[200px] mx-auto">
                <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white mt-7"><?php the_value($item, 'surfaceequipmentserviceitem_title'); ?></h3>
            </div>
        <?php endforeach; ?>
    </div>
</div>
<?php
get_template_part('template-parts/contactbanner');
get_footer();
