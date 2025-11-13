<div class="ani-item flex items-center justify-center flex-wrap gap-2 lg:gap-x-3 mt-[60px]">
    <?php if ($paged > 1) : ?>
        <a href="<?php echo $nav_link($paged - 1); ?>" class="flex size-[48px] border border-[#22313B] rounded-full items-center justify-center text-[16px] text-primary-300 leading-175 transition-colors hover:bg-[#22313B]">
            <svg width="18" height="18" viewBox="0 0 18 18" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path fill-rule="evenodd" clip-rule="evenodd" d="M16.5937 8.99995C16.5937 8.77617 16.5048 8.56156 16.3466 8.40333C16.1884 8.24509 15.9738 8.1562 15.75 8.1562L4.28624 8.1562L7.34624 5.0962C7.49528 4.93625 7.57642 4.7247 7.57256 4.50611C7.56871 4.28752 7.48015 4.07897 7.32556 3.92438C7.17098 3.76979 6.96242 3.68123 6.74383 3.67738C6.52524 3.67352 6.31369 3.75466 6.15374 3.9037L1.65374 8.4037C1.49573 8.5619 1.40698 8.77636 1.40698 8.99995C1.40698 9.22354 1.49573 9.438 1.65374 9.5962L6.15374 14.0962C6.31369 14.2452 6.52524 14.3264 6.74383 14.3225C6.96242 14.3187 7.17098 14.2301 7.32557 14.0755C7.48015 13.9209 7.56871 13.7124 7.57256 13.4938C7.57642 13.2752 7.49528 13.0636 7.34624 12.9037L4.28624 9.8437L15.75 9.8437C15.9738 9.8437 16.1884 9.7548 16.3466 9.59657C16.5048 9.43834 16.5937 9.22373 16.5937 8.99995Z" fill="#A0CDE4" />
            </svg>
        </a>
    <?php endif; ?>

    <?php for ($i = $start; $i <= $end; $i++) : ?>
        <a href="<?php echo $nav_link($i); ?>" class="flex size-[48px] border border-[#22313B] rounded-full items-center justify-center text-[16px] text-primary-300 leading-175 transition-colors <?php echo ($paged == $i ? 'bg-[#22313B]' : 'hover:bg-[#22313B]'); ?>"><?php echo $i; ?></a>
    <?php endfor; ?>
    
    <?php if ($paged < $total) : ?>
        <a href="<?php echo $nav_link($paged + 1); ?>" class="flex size-[48px] border border-[#22313B] rounded-full items-center justify-center text-[16px] text-primary-300 leading-175 transition-colors hover:bg-[#22313B]">
            <svg width="18" height="18" viewBox="0 0 18 18" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path fill-rule="evenodd" clip-rule="evenodd" d="M1.40626 8.99995C1.40626 8.77617 1.49516 8.56156 1.65339 8.40333C1.81162 8.24509 2.02623 8.1562 2.25001 8.1562L13.7138 8.1562L10.6538 5.0962C10.5047 4.93625 10.4236 4.7247 10.4274 4.50611C10.4313 4.28752 10.5198 4.07897 10.6744 3.92438C10.829 3.76979 11.0376 3.68123 11.2562 3.67738C11.4748 3.67352 11.6863 3.75466 11.8463 3.9037L16.3463 8.4037C16.5043 8.5619 16.593 8.77636 16.593 8.99995C16.593 9.22354 16.5043 9.438 16.3463 9.5962L11.8463 14.0962C11.6863 14.2452 11.4748 14.3264 11.2562 14.3225C11.0376 14.3187 10.829 14.2301 10.6744 14.0755C10.5198 13.9209 10.4313 13.7124 10.4274 13.4938C10.4236 13.2752 10.5047 13.0636 10.6538 12.9037L13.7138 9.8437L2.25001 9.8437C2.02623 9.8437 1.81162 9.7548 1.65339 9.59657C1.49516 9.43834 1.40626 9.22373 1.40626 8.99995Z" fill="#A0CDE4" />
            </svg>
        </a>
    <?php endif; ?>
</div>