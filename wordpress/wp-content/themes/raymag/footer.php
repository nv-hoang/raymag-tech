<?php

/**
 * The template for displaying the footer
 *
 * Contains the closing of the #content div and all content after.
 *
 * @link https://developer.wordpress.org/themes/basics/template-files/#template-partials
 *
 * @package Raymag
 */

?>

<!-- Breadcrumb -->
<div class="border-t border-primary-300 border-opacity-20">
	<div class="px-3 lg:px-10">
		<div class="mx-auto max-w-[1600px]">
			<div class="flex items-end flex-wrap gap-x-3 py-3">
				<a href="<?php echo esc_url(home_url('/')); ?>" class="text-[16px] leading-175 text-gray-100 transition-colors hover:text-primary-300"><?php the_trans('Home'); ?></a>
				<?php if (false): ?>
					<svg width="6" height="28" viewBox="0 0 6 28" fill="none" xmlns="http://www.w3.org/2000/svg">
						<path d="M5.34091 7.81818L1.59091 21.75H0.363636L4.11364 7.81818H5.34091Z" fill="#A0CDE4" />
					</svg>
					<a href="#" class="text-[16px] leading-175 text-gray-100 transition-colors hover:text-primary-300">Level 2</a>
					<svg width="6" height="28" viewBox="0 0 6 28" fill="none" xmlns="http://www.w3.org/2000/svg">
						<path d="M5.34091 7.81818L1.59091 21.75H0.363636L4.11364 7.81818H5.34091Z" fill="#A0CDE4" />
					</svg>
					<a href="#" class="text-[16px] leading-175 text-gray-100 transition-colors hover:text-primary-300">Level 1</a>
				<?php endif; ?>
			</div>
		</div>
	</div>
</div>

<!-- Footer -->
<div class="border-t border-primary-300 border-opacity-20">
	<div class="px-3 lg:px-10">
		<div class="mx-auto max-w-[1600px] py-10">
			<div class="grid grid-cols-1 lg:grid-cols-2 gap-10">
				<div class="lg:flex flex-col justify-between">
					<div>
						<img src="<?php the_theme_option('general.logo'); ?>" class="h-[48px]">
					</div>
					<div class="hidden lg:block">
						<div class="text-[16px] leading-175 text-gray-500"><?php the_theme_option('footer.copyright', true); ?></div>
					</div>
				</div>

				<div class="grid grid-cols-1 lg:grid-cols-2 gap-10">
					<div class="flex flex-col gap-3">
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

					<div class="flex flex-col gap-3">
						<?php foreach (get_wp_menu('footer-menu') as $menu): ?>
							<a href="<?php echo $menu->url; ?>" <?php echo ($menu->url == '#contact' ? 'data-anime="scroll-to" data-target="#section-contact"':''); ?> class="text-[16px] leading-175 text-gray-100 hover:text-primary-300 transition-colors"><?php echo $menu->title; ?></a>
						<?php endforeach; ?>
					</div>
				</div>

				<div class="lg:hidden text-[16px] leading-175 text-gray-500"><?php the_theme_option('footer.copyright', true); ?></div>
			</div>
		</div>
	</div>
</div>

</div> <!-- end of container 1920px -->
</div> <!-- end of smooth-content -->
</div> <!-- end of smooth-wrapper -->

<?php wp_footer(); ?>

</body>

</html>