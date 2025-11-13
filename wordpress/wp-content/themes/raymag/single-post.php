<?php
/**
 * The template for displaying all single posts
 *
 * @link https://developer.wordpress.org/themes/basics/template-hierarchy/#single-post
 *
 * @package Raymag
 */

$page_news = get_template_page('raymag_template_news');
get_header();
?>

<div data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
    <div class="relative">
        <img src="<?php echo $page_news ? get_custom_field('news_kv.newskv_image', get_theme_asset_url('assets/img/bg-news-heading.webp'), $page_news->ID) : get_theme_asset_url('assets/img/bg-news-heading.webp'); ?>" class="absolute inset-0 w-full h-full object-cover">
        <div class="absolute inset-0 w-full h-full" style="background: linear-gradient(180deg, rgba(3, 10, 17, 0.5) 0%, #030A11 100%);"></div>
        <div class="relative z-10 max-w-[768px] mx-auto px-3 lg:px-10 text-center py-[100px] lg:py-[200px]">
            <div class="ani-item text-[32px] lg:text-[40px] leading-150 2xl:leading-125 font-medium text-primary-300"><?php echo ($page_news ? $page_news->post_title : '最新消息'); ?></div>
        </div>
    </div>

    <div class="max-w-[1280px] mx-auto px-3 lg:px-10 py-[100px]">
        <div class="ani-item card-1 py-10 px-3 lg:px-10">
            <div class="text-right text-[16px] text-primary-300 leading-175"><?php echo get_the_date('F j, Y'); ?></div>
            <h1 class="text-center mt-3 text-[32px] lg:text-[40px] leading-150 lg:leading-125 font-medium text-white"><?php the_title(); ?></h1>
            <div class="my-10 border-t border-opacity-20 border-primary-300"></div>

            <div class="editor-content-style">
                <?php the_content(); ?>
            </div>
        </div>

        <div class="ani-item mt-10 flex flex-col sm:flex-row items-center justify-between gap-7">
            <div class="flex items-center gap-3">
                <div class="text-gray-100 text-[16px] leading-175">Share :</div>
                <div>
                    <div onclick="sharePost('line')" class="rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative w-full">
                        <div class="size-[48px] flex items-center justify-center relative">
                            <img src="<?php the_theme_asset_url('assets/img/icon-share-line.svg'); ?>" alt="">
                        </div>
                    </div>
                </div>

                <div>
                    <div onclick="sharePost('facebook')" class="rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative w-full">
                        <div class="size-[48px] flex items-center justify-center relative">
                            <img src="<?php the_theme_asset_url('assets/img/icon-share-fb.svg'); ?>" alt="">
                        </div>
                    </div>
                </div>

                <div>
                    <div onclick="sharePost('copy')" class="rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative w-full">
                        <div class="size-[48px] flex items-center justify-center relative">
                            <img src="<?php the_theme_asset_url('assets/img/icon-share-link.svg'); ?>" alt="">
                        </div>
                    </div>
                </div>
            </div>

            <div>
                <a href="<?php echo get_permalink($page_news->ID); ?>" class="inline-block rounded-full overflow-hidden gradient-btn-1 cursor-pointer relative">
                    <div class="inline-flex items-center gap-x-[10px] px-10 py-[10px] relative">
                        <div class="text-[16px] leading-175 font-medium text-white">Back List</div>
                        <img src="<?php the_theme_asset_url('assets/img/icon-arrow.svg'); ?>">
                    </div>
                </a>
            </div>
        </div>
    </div>
</div>

<script>
    function sharePost(platform) {
        var url = window.location.href;

        const dualScreenLeft = window.screenLeft !== undefined ? window.screenLeft : window.screenX;
        const dualScreenTop = window.screenTop !== undefined ? window.screenTop : window.screenY;

        const width = window.innerWidth
            ? window.innerWidth
            : document.documentElement.clientWidth
                ? document.documentElement.clientWidth
                : screen.width;

        const height = window.innerHeight
            ? window.innerHeight
            : document.documentElement.clientHeight
                ? document.documentElement.clientHeight
                : screen.height;

		var left = (width - 600) / 2 + dualScreenLeft;
        var top = (height - 500) / 2 + dualScreenTop;

        switch (platform) {
            case 'linkedin':
                var left = (width - 600) / 2 + dualScreenLeft;
                var top = (height - 500) / 2 + dualScreenTop;
                window.open(
                    "https://www.linkedin.com/sharing/share-offsite/?url=" + encodeURIComponent(url),
                    "_blank",
                    "width=600,height=500, top=" + top + ", left=" + left
                );
                break;

			case 'facebook':
				window.open(
                    'https://www.facebook.com/sharer/sharer.php?u=' + encodeURIComponent(url),
                    "_blank",
                    "width=600,height=500, top=" + top + ", left=" + left
                );
				break;

			case 'line':
				window.open(
                    'https://social-plugins.line.me/lineit/share?url=' + encodeURIComponent(url),
                    "_blank",
                    "width=600,height=500, top=" + top + ", left=" + left
                );
				break;

            case 'copy':
                navigator.clipboard.writeText(url).then(() => {
                    Toastify('連結已複製到剪貼簿！');
                }).catch(err => {
                    console.error("Copy failed", err);
                });
                break;

            default:
                console.warn("Unsupported platform:", platform);
        }
    }
</script>

<?php 
	$cats = wp_get_post_categories(get_the_ID());
	$tags = wp_get_post_tags(get_the_ID());
	$tag_ids = wp_list_pluck($tags, 'term_id');

	$args = array(
		'post__not_in'   => array(get_the_ID()),
		'posts_per_page' => 2,
		'tax_query'      => array(
			'relation' => 'OR',
			array(
				'taxonomy' => 'category',
				'field'    => 'term_id',
				'terms'    => $cats,
			),
			array(
				'taxonomy' => 'post_tag',
				'field'    => 'term_id',
				'terms'    => $tag_ids,
			),
		),
	);
	$related = new WP_Query($args);
?>
<?php if($related->have_posts()): ?>
	<div class="max-w-[1280px] mx-auto px-3 lg:px-10 py-[100px]">
		<div data-ani="fadeUp" data-delay="0.4" data-target=".ani-item">
			<h2 class="ani-item text-[24px] lg:text-[32px] leading-150 font-medium text-primary-300 text-center">推薦文章</h2>
	
			<div class="mt-10">
				<?php 
				while ($related->have_posts()) : 
					$related->the_post();
				?>
					<div class="ani-item">
						<a href="<?php echo get_permalink(); ?>" class="flex flex-col lg:flex-row gap-3 lg:gap-10 hover-scale">
							<div class="lg:w-[320px]">
								<div class="inline-block w-full h-[210px] lg:h-[200px] overflow-hidden rounded-[8px]">
									<?php if (has_post_thumbnail()) : ?>
										<img src="<?php the_post_thumbnail(); ?>" class="w-full h-full object-cover transition-transform">
									<?php else : ?>
										<img src="<?php the_theme_asset_url('assets/img/pic-home-news-2.webp'); ?>" class="w-full h-full object-cover transition-transform">
									<?php endif; ?>
									
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
			</div>
		</div>
	</div>
	<?php wp_reset_postdata(); ?>
<?php endif; ?>
<?php
get_template_part('template-parts/contactbanner');
get_footer();
