<?php
/**
 * The base configuration for WordPress
 *
 * The wp-config.php creation script uses this file during the installation.
 * You don't have to use the website, you can copy this file to "wp-config.php"
 * and fill in the values.
 *
 * This file contains the following configurations:
 *
 * * Database settings
 * * Secret keys
 * * Database table prefix
 * * ABSPATH
 *
 * @link https://developer.wordpress.org/advanced-administration/wordpress/wp-config/
 *
 * @package WordPress
 */

// ** Database settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define( 'DB_NAME', 'wordpress' );

/** Database username */
define( 'DB_USER', 'wordpress' );

/** Database password */
define( 'DB_PASSWORD', '' );

/** Database hostname */
define( 'DB_HOST', 'mariadb' );

/** Database charset to use in creating database tables. */
define( 'DB_CHARSET', 'utf8mb4' );

/** The database collate type. Don't change this if in doubt. */
define( 'DB_COLLATE', '' );

/**#@+
 * Authentication unique keys and salts.
 *
 * Change these to different unique phrases! You can generate these using
 * the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}.
 *
 * You can change these at any point in time to invalidate all existing cookies.
 * This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
define( 'AUTH_KEY',         'EyJ%O3#mGdX=k}^03bO2Hm  Pj&&<]UZGwUG`G8g1Z-l*-L3GM[ryr5FsI16SA%6' );
define( 'SECURE_AUTH_KEY',  '17fvp) C:OFHiW!.|hk[.!!,fVqblNp]20kR`OI/&5c,divuL]ip{,O&??!s3S{)' );
define( 'LOGGED_IN_KEY',    'k,=}!{$6P9pz?r}dtFwdzis?-.MT2K-R 8q.|*>aY70ZF6XGff1[)$h4=nUc.t2u' );
define( 'NONCE_KEY',        ')w_jSvVQa}$vbUH3F5-?@M3d{S5+`{dg]>:R3:!p%h?;@spiXYJuB.Jq*)/Frwiz' );
define( 'AUTH_SALT',        '8KJsd-NU+5_P/tqPy<@t}vU+@KIYY<{*0dN2/3FJ7 EjJ-Z0dvTbb|>F8qY:^V~S' );
define( 'SECURE_AUTH_SALT', 'DNo~(vySj#b>VApDC:)c#f(CqHdG}`|l]cdg*vsclU3fVm);.nf`1l%-S>58=N1x' );
define( 'LOGGED_IN_SALT',   'S2:HtSV#a#i!7`s*SWBqx6BxOJj{k$!.{T5:]HV^;,[M!W)q/`w*PgGF> CqxZ$x' );
define( 'NONCE_SALT',       ']=PU`WVN.@RL1TKlxJJEH|$EkD~Alqz8|]y5S+d}k@y@u|Dn0H2g$ JKwJ:pvL!y' );

/**#@-*/

/**
 * WordPress database table prefix.
 *
 * You can have multiple installations in one database if you give each
 * a unique prefix. Only numbers, letters, and underscores please!
 *
 * At the installation time, database tables are created with the specified prefix.
 * Changing this value after WordPress is installed will make your site think
 * it has not been installed.
 *
 * @link https://developer.wordpress.org/advanced-administration/wordpress/wp-config/#table-prefix
 */
$table_prefix = 'wp_';

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 *
 * For information on other constants that can be used for debugging,
 * visit the documentation.
 *
 * @link https://developer.wordpress.org/advanced-administration/debug/debug-wordpress/
 */
define( 'WP_DEBUG', false );

/* Add any custom values between this line and the "stop editing" line. */



/* That's all, stop editing! Happy publishing. */

/** Absolute path to the WordPress directory. */
if ( ! defined( 'ABSPATH' ) ) {
	define( 'ABSPATH', __DIR__ . '/' );
}

/** Sets up WordPress vars and included files. */
require_once ABSPATH . 'wp-settings.php';
