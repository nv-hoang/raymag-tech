<?php

require_once(__DIR__ . '/init.php');

function skip_plugin_update($transient)
{
    global $wp_version;
    $excluded = [
        'advanced-custom-fields-pro',
        'wpforms',
    ];

    if (isset($transient->response)) {
        $transient->response = array_filter($transient->response, fn($plugin) => !in_array($plugin->slug, $excluded));
        if (empty($transient->response)) {
            return (object) array('last_checked' => time(), 'version_checked' => $wp_version);
        }
    }
    return $transient;
}
add_filter('site_transient_update_plugins', 'skip_plugin_update');

// function wpdocs_theme_add_editor_styles()
// {
//     add_editor_style(get_theme_asset_url('css/custom-editor-style.css'));
// }
// add_action('admin_init', 'wpdocs_theme_add_editor_styles');