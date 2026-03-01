import { Component, computed, signal, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { User } from '../../interfaces/user';

type ViewMode = 'grid' | 'table';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit {
  private userService = inject(UserService);

  users = signal<User[]>([]);
  isLoading = signal<boolean>(true);
  error = signal<string | null>(null);

  selectedProject = signal<string>('ALL');
  viewMode = signal<ViewMode>('grid');

  filteredUsers = computed(() => {
    const project = this.selectedProject();
    const currentUsers = this.users();
    if (project === 'ALL') return currentUsers;
    return currentUsers.filter((u) => u.project === project);
  });

  uniqueProjects = computed(() => {
    return Array.from(new Set(this.users().map((u) => u.project))).sort();
  });

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.isLoading.set(true);
    this.error.set(null);

    this.userService.getUsers().subscribe({
      next: (apiUsers) => {
        // Map API model to Component model
        const mappedUsers = apiUsers.map((apiUser) => {
          // Calculate an active status based on last connected time
          // Here we just make it simple: if lastConnected is recent enough, it's active.
          // For demo purposes, we will treat everyone as active unless logic dictates otherwise.
          const lastConnectionDate = new Date(apiUser.lastConnected);
          const now = new Date();
          const inactiveThresholdHours = 2; // threshold for offline
          const hoursDifference = Math.abs(now.getTime() - lastConnectionDate.getTime()) / 3600000;

          return {
            id: apiUser.id,
            name: apiUser.name || 'Unknown User',
            project: apiUser.project || 'Unassigned',
            ipAddress: apiUser.ipAddress || 'Not Assigned',
            password: apiUser.password || '******',
            grubPassword: apiUser.grubPassword || '******',
            ailTag: apiUser.ailTag || "",
            hostname: apiUser.hostname || "",
            status: hoursDifference > inactiveThresholdHours ? 'offline' : 'active',
            lastConnected: apiUser.lastConnected,
          } as User;
        });

        this.users.set(mappedUsers);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load users', err);
        // Fallback to dummy data if local API fails specifically for the demo
        this.isLoading.set(false);
        this.error.set(
          'Failed to connect to the backend server at localhost:5257. Make sure the API is running.',
        );
      },
    });
  }

  setProject(project: string) {
    this.selectedProject.set(project);
  }

  setViewMode(mode: ViewMode) {
    this.viewMode.set(mode);
  }

  getProjectCount(project: string): number {
    return this.users().filter((u) => u.project === project).length;
  }

  getActiveCount(): number {
    return this.filteredUsers().filter((u) => u.status === 'active').length;
  }

  getOfflineCount(): number {
    return this.filteredUsers().filter((u) => u.status === 'offline').length;
  }

  formatLastConnected(dateString: string): string {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return dateString; // fallback if invalid

    // Formatting logic similar to relative time
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHrs = Math.floor(diffMins / 60);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins} mins ago`;
    if (diffHrs < 24) return `${diffHrs} hours ago`;
    return date.toLocaleDateString();
  }

  copyToClipboard(event: Event, text: string) {
    if (!navigator.clipboard) {
      console.warn('Clipboard API not available');
      return;
    }
    navigator.clipboard
      .writeText(text)
      .then(() => {
        const target = event.target as HTMLElement;
        const originalText = target.innerText;
        target.innerText = 'Copied!';
        target.style.color = '#10b981'; // Success green text
        setTimeout(() => {
          target.innerText = originalText;
          target.style.color = ''; // Revert to default
        }, 1500);
      })
      .catch((err) => {
        console.error('Failed to copy text: ', err);
      });
  }
}
