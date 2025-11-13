<?php

/**
 * Helpers
 *
 * @link https://developer.wordpress.org/themes/basics/theme-functions/
 *
 * @package 
 */

function get_theme_languages($exclude_current = false)
{
    if (class_exists('pll_the_languages')) {
        $languages = array_values(pll_the_languages(['raw' => 1]));
        if ($exclude_current) {
            $languages = array_filter($languages, fn($lang) => !$lang['current_lang']);
        }
        return $languages;
    }
    return [];
}

function get_current_lang($key = null)
{
    $current_language = array_filter(get_theme_languages(), fn($lang) => $lang['current_lang']);
    if (empty($current_language)) {
        $current_language = [
            [
                'slug' => 'tw',
                'name' => '',
                'url' => ''
            ]
        ];
    }
    $lang = end($current_language);
    return $key ? $lang[$key] : $lang;
}

function get_theme_option($key, $lang = false, $default = '')
{
    if ($lang) $key .= '.' . get_current_lang('slug');
    $field = get_field(str_replace('.', '_', $key), 'option');

    if (empty($field)) {
        $field = $default;
    }
    return is_string($field) && !isHtml($field) ? nl2br($field) : $field;
}

function the_theme_option($key, $lang = false, $default = '')
{
    echo get_theme_option($key, $lang, $default);
}

function isHtml($string)
{
    return $string !== strip_tags($string);
}

function get_custom_field($key, $default = '', $post_id = false)
{
    $field = get_field(str_replace('.', '_', $key), $post_id);
    if (empty($field)) {
        $field = $default;
    }
    return is_string($field) && !isHtml($field) ? nl2br($field) : $field;
}

function the_custom_field($key, $default = '', $post_id = false)
{
    echo get_custom_field($key, $default, $post_id);
}

function get_value($value, $key, $default = '')
{
    $keys = explode('.', $key, 2);
    if (!empty($value) && isset($value[$keys[0]])) {
        if (count($keys) >= 2) return get_value($value[$keys[0]], $keys[1]);
        if (!empty($value[$key])) return is_string($value[$key]) && !isHtml($value[$key]) ? nl2br($value[$key]) : $value[$key];
    }
    return is_string($default) && !isHtml($default) ? nl2br($default) : $default;
}

function the_value($value, $key, $default = '')
{
    echo get_value($value, $key, $default);
}

function menu_to_tree(array &$elements, $parentId = 0)
{
    $branch = array();
    foreach ($elements as &$element) {
        if ($element->menu_item_parent == $parentId) {
            $element->childrens = menu_to_tree($elements, $element->ID);
            $element->menu_desc = get_custom_field('menu_desc', '', $element->ID);
            $element->is_mega_menu = get_custom_field('is_mega_menu', false, $element->ID);
            $branch[$element->ID] = $element;
            unset($element);
        }
    }
    return $branch;
}

function get_wp_menu($menu_name = null)
{
    if ($menu_name == null) {
        $menus = get_nav_menu_locations();
        if (count($menus)) {
            $menu_items = wp_get_nav_menu_items(array_shift($menus));
            return menu_to_tree($menu_items);
        } else return [];
    } else {
        $menu_name .= '-' . get_current_lang('slug');
        $menu_items = wp_get_nav_menu_items($menu_name);
        if ($menu_items === false) return [];
        return menu_to_tree($menu_items);
    }
}

function get_theme_asset_url($path)
{
    $path = ltrim($path, '/');
    return get_template_directory_uri() . "/styles/dist/{$path}?ver=" . _S_VERSION;
}

function the_theme_asset_url($path)
{
    echo get_theme_asset_url($path);
}

function get_request_parameter($key, $default = '')
{
    // If not request set
    if (!isset($_REQUEST[$key]) || empty($_REQUEST[$key])) {
        return $default;
    }
    // Set so process it
    return wp_unslash($_REQUEST[$key]);
}

function get_template_page($template_name)
{
    $pages = query_posts(array(
        'meta_key'    => '_wp_page_template',
        'meta_value'    => "{$template_name}.php",
        'post_type'   => 'page',
        'post_status' => 'publish',
        'numberposts' => 1
    ));
    wp_reset_query();
    if ($pages === false) return null;
    return end($pages);
}

function get_template_page_title($template_name)
{
    $page = get_template_page($template_name);
    return $page ? $page->post_title : '';
}

function the_template_page_title($template_name)
{
    echo get_template_page_title($template_name);
}

function get_template_page_url($template_name, $default = '')
{
    $page = get_template_page($template_name);
    return $page ? get_page_link($page->ID) : $default;
}

function the_template_page_url($template_name)
{
    echo get_template_page_url($template_name, home_url('/'));
}

function get_textarea_custom_field($key, $separator = '<br />')
{
    return explode($separator, get_custom_field($key));
}

function get_map_popup_content($key)
{
    $html = '';
    foreach (get_textarea_custom_field($key, "<br />\r\n<br />") as $item) {
        if (strpos($item, '<br />') === false) {
            $html .= '<div class="text-[16px] leading-175 mt-3">' . $item . '</div>';
        } else {
            $items = explode('<br />', $item);
            $html .= '<div class="text-[18px] font-medium leading-175 lg:text-[20px] lg:leading-150 mt-3">' . $items[0] . '</div>';
            $html .= '<div class="text-[16px] leading-175 mt-1">' . $items[1] . '</div>';
        }
    }
    return $html;
}

function replace_image_src_from_publishing($html)
{
    $baseURL = "<?php the_theme_asset_url('assets/img/";
    $pattern = '/src="assets\/img\/(.*?)"/';
    $replacement = 'src="' . $baseURL . '$1\'); ?>"';
    $result = preg_replace($pattern, $replacement, $html);

    return $result;
}

function replace_background_url_from_publishing($html)
{
    $baseURL = "<?php the_theme_asset_url('assets/img/";
    $pattern = '/background-image: url\(\'assets\/img\/(.*?)\'\);/';
    $replacement = 'background-image: url(\'' . $baseURL . '$1\'); ?>\');';
    $result = preg_replace($pattern, $replacement, $html);

    return $result;
}

function create_page_from_publishing($page)
{
    if (file_exists($page)) {
        $html = file_get_contents($page);
        file_put_contents($page, replace_image_src_from_publishing(replace_background_url_from_publishing($html)));
        echo 'OK';
        exit();
    } else {
        echo 'Not found!.';
        exit();
    }
}

function dd($data)
{
    echo '<pre>';
    var_dump($data);
    echo '</pre>';
    die();
}

function trans($single, $plural = null, $number = null)
{
    if ($plural === null) {
        return __($single, _LANG_DOMAIN);
    }
    return _n($single, $plural, $number, _LANG_DOMAIN);
}

function the_trans($single, $plural = null, $number = null)
{
    echo trans($single, $plural, $number);
}

function the_paginate($paged, $max_num_pages, $params = [], $use_pagenum_link = false)
{
    // paginate
    $total = $max_num_pages;
    $len = 3;
    $offset = floor($len / 2);
    $start = max($paged - $offset, 1);
    $end = min(min($paged + $offset, $total) + max($offset - ($paged - $start), 0), $total);
    $start = max($start - max($offset - ($end - $paged), 0), 1);

    $nav_link = function ($p) use ($params, $use_pagenum_link) {
        if ($use_pagenum_link) {
            $url = get_pagenum_link($p, true);
            if (empty($params)) return $url;
            return $url . (strpos($url, '?') === false ? '?' : '&') . http_build_query($params);
        }
        $params['cpage'] = $p;
        return '?' . http_build_query($params);
    };

    include locate_template('template-parts/layout-paginate.php');
}
