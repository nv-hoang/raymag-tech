<?php

/**
 * The header for our theme
 *
 * This is the template that displays all of the <head> section and everything up until <div id="content">
 *
 * @link https://developer.wordpress.org/themes/basics/template-files/#template-partials
 *
 * @package Raymag
 */

?>
<!doctype html>
<html <?php language_attributes(); ?>>

<head>
	<meta charset="UTF-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">

	<?php wp_head(); ?>

	<script type="module" crossorigin src="<?php the_theme_asset_url('assets/main.js'); ?>"></script>
	<link rel="stylesheet" crossorigin href="<?php the_theme_asset_url('assets/main.css'); ?>">

	<style>
		#wpadminbar {
			position: fixed !important;
		}
		body.admin-bar .header-container {
			top: 32px !important;
		}
		@media screen and (max-width: 782px) {
			body.admin-bar .header-container {
				top: 46px !important;
			}
		}
	</style>
</head>

<body <?php body_class(); ?> style="background-color: #030A11; color: #fff; overflow-x: hidden;">
	<?php wp_body_open(); ?>
	<?php get_template_part('template-parts/menu'); ?>

	<div class="fixed-container mx-auto relative"></div>
	<div id="smooth-wrapper">
		<div id="smooth-content">
			<div class="overflow-hidden">
				<div class="h-[50px] lg:h-[66px] 2xl:h-[68px]"></div>