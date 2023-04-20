import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlogComponent } from './blog/blog.component';
import { CulturesComponent } from './cultures/cultures.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MyScoreComponent } from './my-score/my-score.component';
import { MyStoriesComponent } from './my-stories/my-stories.component';
import { SettingsComponent } from './settings/settings.component';

const routes: Routes = [
  { path: '',redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard',component:DashboardComponent},
  { path: 'cultures',component:CulturesComponent},
  { path: 'my-stories',component:MyStoriesComponent},
  { path: 'my-score',component:MyScoreComponent},
  { path: 'blog',component:BlogComponent},
  { path: 'settings',component:SettingsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
