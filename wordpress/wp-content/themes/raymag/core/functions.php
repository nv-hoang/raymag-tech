<?php

require_once(__DIR__ . '/init.php');

function hide_update_notifications()
{
    global $wp_version;
    return (object) array('last_checked' => time(), 'version_checked' => $wp_version);
}
if (defined('HIDE_UPDATE_NOTIFICATIONS') && HIDE_UPDATE_NOTIFICATIONS) {
    add_filter('pre_site_transient_update_core', 'hide_update_notifications'); //hide updates for WordPress itself
    add_filter('pre_site_transient_update_plugins', 'hide_update_notifications'); //hide updates for all plugins
    add_filter('pre_site_transient_update_themes', 'hide_update_notifications'); //hide updates for all themes
}

// function wpdocs_theme_add_editor_styles()
// {
//     add_editor_style(get_theme_asset_url('css/custom-editor-style.css'));
// }
// add_action('admin_init', 'wpdocs_theme_add_editor_styles');