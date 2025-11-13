<?php

/**
 * Template Name: Raymag Template - News
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

// $per_page = get_option('posts_per_page');
$per_page = 6;

$paged = 1;
if (isset($_GET['cpage'])) {
    $paged = intval($_GET['cpage']);
}

$args = array(
    'post_type' => 'post',
    'paged' => $paged,
    'posts_per_page' => $per_page,
    'orderby' => 'date',
    'order' => 'DESC',
    'post_status' => 'publish',
);

// $search_query = isset($_GET['search']) ? sanitize_text_field($_GET['search']) : '';
// if (!empty($search_query)) {
//     $args['s'] = $search_query;
// }

$query = new WP_Query($args);
$idx = 0;
get_header();
?>

<div data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <div class="relative">
        <img src="<?php the_custom_field('news_kv.newskv_image'); ?>" class="absolute inset-0 w-full h-full object-cover">
        <div class="absolute inset-0 w-full h-full" style="background: linear-gradient(180deg, rgba(3, 10, 17, 0.5) 0%, #030A11 100%);"></div>
        <div class="relative z-10 max-w-[768px] mx-auto px-3 lg:px-10 text-center py-[100px] lg:py-[200px]">
            <h1 class="ani-item text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-primary-300"><?php the_title(); ?></h1>
        </div>
    </div>

    <?php if ($query->have_posts()) : ?>
    <div class="max-w-[1024px] mx-auto px-3 lg:px-10 py-[100px]">
        <?php while ($query->have_posts()) : ?>
            <?php
                $idx++;
                $query->the_post();
                $thumbnail = get_the_post_thumbnail_url(get_the_ID(), 'full');
                if (empty($thumbnail)) {
                    $thumbnail = get_theme_asset_url('assets/img/pic-home-news-2.webp');
                }
            ?>
            <div class="ani-item <?php echo ($idx % 2 == 0 ? 'mt-7 pt-7 border-t border-primary-300 border-opacity-20':''); ?>">
                <a href="<?php echo get_permalink(); ?>" class="flex flex-col lg:flex-row gap-3 lg:gap-10 hover-scale">
                    <div class="lg:w-[320px]">
                        <div class="inline-block w-full h-[210px] lg:h-[200px] overflow-hidden rounded-[8px]">
                            <img src="<?php echo $thumbnail; ?>" class="w-full h-full object-cover transition-transform">
                        </div>
                    </div>
                    <div class="flex-1 px-3 lg:p-3">
                        <div class="text-[16px] leading-175 text-primary-300 text-right"><?php echo get_the_date('F j, Y'); ?></div>
                        <h3 class="text-[20px] lg:text-[24px] leading-150 font-medium text-white mt-3"><?php the_title(); ?></h3>
                        <p class="text-[16px] leading-175 text-gray-100 mt-3"><?php echo get_the_excerpt(); ?></p>
                    </div>
                </a>
            </div>
        <?php endwhile; ?>

        <?php 
            the_paginate($paged, $query->max_num_pages);
            wp_reset_postdata();
        ?>
    </div>
    <?php endif; ?>
</div>
<?php
get_footer();
