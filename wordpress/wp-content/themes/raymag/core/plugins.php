<?php

/**
 * Plugin activation
 *
 * @link https://developer.wordpress.org/themes/basics/theme-functions/
 *
 * @package
 */

function theme_plugin_activation()
{
    $plugins = array(
        array(
            'name' => 'Advanced Custom Fields Pro',
            'slug' => 'advanced-custom-fields-pro',
            'required' => true
        ),
    );

    $configs = array(
        'menu' => 'tp_plugin_install',
        'has_notice' => true,
        'dismissable' => false,
        'is_automatic' => true
    );

    tgmpa($plugins, $configs);
}
add_action('tgmpa_register', 'theme_plugin_activation');
