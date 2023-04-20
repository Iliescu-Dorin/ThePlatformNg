export const languages = [
  {
    language: 'English',
    code: 'en',
    flag: 'assets/flags/us.jpg',
    rtl: false
  },
  {
    language: 'French',
    code: 'fr',
    flag: 'assets/flags/fr.jpg',
    rtl: false
  },
  {
    language: 'German',
    code: 'de',
    flag: 'assets/flags/de.jpg',
    rtl: false
  },
  {
    language: 'Russian',
    code: 'ru',
    flag: 'assets/flags/ru.jpg',
    rtl: true
  },
  {
    language: 'Spanish',
    code: 'es',
    flag: 'assets/flags/es.jpg',
    rtl: false
  },
  {
    language: 'Romanian',
    code: 'ro',
    flag: 'assets/flags/ro.png',
    rtl: false
  }
];

export const notifications = [
  {
    id: 1,
    title: 'New User Registered',
    description: 'You have 10 unread messages',
    icon: 'fa fa-user-plus',
    time: '9:30 AM',
    color: 'primary'
  },
  {
    id: 2,
    title: 'New Order Received',
    description: 'You have 20 unread messages',
    icon: 'fa fa-cart-plus',
    time: '10:15 AM',
    color: 'success'
  },
  {
    id: 3,
    title: 'New User Registered',
    description: 'You have 5 unread messages',
    icon: 'fa fa-user-plus',
    time: '11:00 AM',
    color: 'danger'
  },
  {
    id: 4,
    title: 'New Order',
    description: 'You have 10 unread messages',
    icon: 'fa fa-cart-plus',
    time: '11:30 AM',
    color: 'warning'
  },
];

export const userItems = [
  {
    id: 1,
    title: 'My Profile',
    icon: 'fa fa-user',
    link: '/pages/profile'
  },
  {
    id: 2,
    title: 'Inbox',
    icon: 'fa fa-envelope',
    link: '/pages/inbox'
  },
  {
    id: 3,
    title: 'Settings',
    icon: 'fa fa-cog',
    link: '/pages/settings'
  },
  {
    id: 4,
    title: 'Logout',
    icon: 'fa fa-sign-out',
    link: '/pages/login'
  }
];
