<?php

/**
 * Template Name: Raymag Template - Surf-treat (全方位表面處理技術)
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
    <div class="flex flex-col lg:flex-row gap-10 lg:gap-20">
        <div class="lg:w-[580] lg:max-w-[50%]">
            <img src="<?php the_custom_field('surftreat_info.surftreatinfo_image'); ?>" class="mx-auto" data-ani="zoomIn" data-delay="0.4">
        </div>
        <div class="flex-1 xl:px-10" data-ani="fadeUp" data-delay="0.8">
            <?php 
                $surftreatinfo_content = get_custom_field('surftreat_info.surftreatinfo_content');
                $surftreatinfo_content = str_replace('<h2>', '<h2 class="text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300">', $surftreatinfo_content);
                $surftreatinfo_content = str_replace('<h6>', '<h6 class="text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-5">', $surftreatinfo_content);
                $surftreatinfo_content = str_replace('<h4>', '<h4 class="text-[18px] lg:text-[20px] leading-175 font-medium text-white mt-7">', $surftreatinfo_content);
                $surftreatinfo_content = str_replace('<ul>', '<div class="editor-content-style text-[16px] leading-175 text-gray-100 mt-3" style="--li-mb: 8px;"><ul>', $surftreatinfo_content);
                $surftreatinfo_content = str_replace('</ul>', '</ul></div>', $surftreatinfo_content);
                echo $surftreatinfo_content;
            ?>
        </div>
    </div>
</div>

<div class="max-w-[1440px] mx-auto px-3 lg:px-10 py-[100px]" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <h2 class="ani-item text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center"><?php the_custom_field('surftreat_mpc.surftreatmpc_title'); ?></h2>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-7 mt-10">
        <?php foreach(get_custom_field('surftreat_mpc.surftreatmpc_items', []) as $idx => $item): ?>
            <div class="card-1 px-3 xl:px-10 py-10 ani-item">
                <img src="<?php the_value($item, 'surftreatmpcitem_image'); ?>" class="w-[120px] mx-auto">
                <h3 class="text-[18px] lg:text-[20px] leading-175 lg:leading-150 font-medium text-primary-300 mt-7 text-center"><?php the_value($item, 'surftreatmpcitem_title'); ?></h3>
                <div class="editor-content-style text-[16px] leading-175 text-gray-100 mt-5" style="--li-mb: 8px;">
                    <?php the_value($item, 'surftreatmpcitem_content'); ?>
                </div>
            </div>
        <?php endforeach; ?>
    </div>
</div>

<div class="max-w-[1680px] mx-auto px-3 lg:px-10 py-[100px]" data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <h2 class="ani-item text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center"><?php the_custom_field('surftreat_apps.surftreatapp_title'); ?></h2>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-20 lg:gap-y-10 mt-10">
        <?php foreach(get_custom_field('surftreat_apps.surftreatapp_items', []) as $idx => $item): ?>
            <div class="ani-item grid grid-cols-1 md:grid-cols-2 gap-3 md:gap-10">
                <div>
                    <img src="<?php the_value($item, 'surftreatappitem_image'); ?>" class="rounded-[28px] w-full">
                </div>
                <div class="flex flex-col justify-center">
                    <div>
                        <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'surftreatappitem_title'); ?></h3>
                        <div class="border-t border-primary-300 opacity-20 my-3"></div>
                        <div class="text-[16px] lg:text-[18px] leading-175 text-gray-100"><?php the_value($item, 'surftreatappitem_desc'); ?></div>
                    </div>
                </div>
            </div>
        <?php endforeach; ?>

    </div>
</div>

<div class="max-w-[1440px] mx-auto px-3 lg:px-10 py-[100px]">
    <h2 data-ani="fadeUp" data-delay="0.4" class="text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center"><?php the_custom_field('surftreat_timeline.surftreattimeline_title'); ?></h2>

    <div class="relative pl-[60px] md:pl-0 mt-10 timeline">
        <div class="absolute left-[23px] md:left-[50%] h-full border-l border-[#283943]"></div>
        <div class="absolute left-[23px] md:left-[50%] border-l border-primary-500 timeline-progress"></div>

        <div class="flex flex-col gap-20">
            <?php 
                $items = get_custom_field('surftreat_timeline.surftreattimeline_items', []);
                $cnt = count($items);
            ?>
            <?php foreach($items as $idx => $item): ?>
                <div class="relative timeline-item">
                    <?php if($idx == 0): ?>
                        <div class="absolute -left-[37px] md:left-[50%] h-[83px] md:h-[50%] border-l border-[#030A11]"></div>
                    <?php endif; ?>
                    <?php if($idx == ($cnt-1)): ?>
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
                                    <img src="<?php the_value($item, 'surftreattimelineitem_image'); ?>" class="w-full h-full object-cover">
                                </div>
                            </div>
                        </div>
                        <div class="flex flex-col justify-center">
                            <div class="relative xl:pb-20 timeline-item-info" style="opacity: 0;">
                                <div class="overflow-hidden">
                                    <div class="inline-block text-[64px] lg:text-[100px] leading-100 font-semibold" style="font-family: 'Inter';">
                                        <div class="gradient-2"><?php echo str_pad($idx+1, 2, '0', STR_PAD_LEFT); ?></div>
                                    </div>
                                </div>
                                <div class="-mt-5 pl-10">
                                    <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white"><?php the_value($item, 'surftreattimelineitem_title'); ?></h3>
                                    <div class="text-[16px] lg:text-[18px] leading-175 text-gray-100 mt-3"><?php the_value($item, 'surftreattimelineitem_desc'); ?></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            <?php endforeach; ?>
        </div>
    </div>
</div>

<div class="max-w-[1440px] mx-auto px-3 lg:px-10 py-[100px]" data-ani="fadeUp" data-delay="0.4">
    <?php 
        $rows = get_custom_field('surftreat_table.surftreattable_items', []);
    ?>
    <div class="px-[2px]">
        <table class="w-full bg-[#042639] rounded-[8px] overflow-hidden" cellspacing="0" style="border-collapse: separate; table-layout: fixed;">
            <thead>
                <tr>
                    <?php for($i = 0; $i < 1; $i++): ?>
                        <th class="text-[16px] lg:text-[18px] leading-175 font-medium text-primary-300 px-3 py-2 text-left"><?php the_value($rows[$i], 'surftreattableitem_col1'); ?></th>
                        <th class="text-[16px] lg:text-[18px] leading-175 font-medium text-primary-300 px-3 py-2 text-left"><?php the_value($rows[$i], 'surftreattableitem_col2'); ?></th>
                        <th class="text-[16px] lg:text-[18px] leading-175 font-medium text-primary-300 px-3 py-2 text-left"><?php the_value($rows[$i], 'surftreattableitem_col3'); ?></th>
                    <?php endfor; ?>
                </tr>
            </thead>
        </table>
    </div>

    <table class="w-full" cellspacing="2" style="border-collapse: separate; table-layout: fixed;">
        <thead>
            <?php for($i = 1; $i < count($rows); $i++): ?>
                <tr>
                    <th class="bg-[#031825] rounded-[8px] text-[16px] lg:text-[18px] leading-175 font-medium text-white px-3 py-2 text-left"><?php the_value($rows[$i], 'surftreattableitem_col1'); ?></th>
                    <th class="bg-[#031825] rounded-[8px] text-[16px] lg:text-[18px] leading-175 font-medium text-gray-100 px-3 py-2 text-left"><?php the_value($rows[$i], 'surftreattableitem_col2'); ?></th>
                    <th class="bg-[#031825] rounded-[8px] text-[16px] lg:text-[18px] leading-175 font-medium text-gray-100 px-3 py-2 text-left"><?php the_value($rows[$i], 'surftreattableitem_col3'); ?></th>
                </tr>
            <?php endfor; ?>
        </thead>
    </table>

    <div class="text-[16px] leading-175 text-primary-300 md:text-right mt-3"><?php the_custom_field('surftreat_table.surftreattable_note'); ?></div>
</div>

<?php
get_template_part('template-parts/contactbanner');
get_footer();
