<?php

/**
 * Helpers
 *
 * @link https://developer.wordpress.org/themes/basics/theme-functions/
 *
 * @package 
 */

function get_theme_option($key, $default = '')
{
    $field = get_field(str_replace('.', '_', $key), 'option');
    return empty($field) ? $default : $field;
}

function the_theme_option($key, $default = '')
{
    echo get_theme_option($key, $default);
}

function get_custom_field($key, $default = '', $post_id = false)
{
    $field = get_field(str_replace('.', '_', $key), $post_id);
    return empty($field) ? $default : $field;
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
        return $value[$key];
    }
    return $default;
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
        $menu_items = wp_get_nav_menu_items($menu_name);
        if ($menu_items === false) return [];
        return menu_to_tree($menu_items);
    }
}

function get_theme_asset_url($path)
{
    $path = ltrim($path, '/');
    return get_template_directory_uri() . "/styles/dist/{$path}";
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
    return $pages === false ? null : $pages[0];
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

function get_template_page_url($template_name)
{
    $page = get_template_page($template_name);
    return $page ? get_page_link($page->ID) : home_url('/');
}

function the_template_page_url($template_name)
{
    echo get_template_page_url($template_name);
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
