import { Component, computed, signal, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, ApiUser } from '../../services/user.service';

interface User {
  id: string; // Updated to string for UUID
  name: string;
  project: string;
  ailTag: string; // We'll keep this, maybe generate or adapt it
  ipAddress: string;
  password: string;
  grubPassword: string;
  status: 'active' | 'offline'; // We'll infer this for now
  lastConnected: string;
}

type ViewMode = 'grid' | 'table';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="users-layout">
      <!-- Sidebar -->
      <aside class="sidebar">
        <h3>Filter by Project</h3>
        <nav class="project-nav">
          <button 
            class="nav-item" 
            [class.active]="selectedProject() === 'ALL'"
            (click)="setProject('ALL')"
          >
            <span class="project-name">ALL</span>
            <span class="count-badge">{{ users().length }}</span>
          </button>
          
          @for (project of uniqueProjects(); track project) {
            <button 
              class="nav-item"
              [class.active]="selectedProject() === project"
              (click)="setProject(project)"
            >
              <span class="project-name">{{ project }}</span>
              <span class="count-badge">{{ getProjectCount(project) }}</span>
            </button>
          }
        </nav>
      </aside>

      <!-- Main Content -->
      <main class="users-main">
        <div class="users-header">
          <div class="header-left">
            <h2>{{ selectedProject() === 'ALL' ? 'All System Users' : selectedProject() + ' Users' }}</h2>
            <div class="stats">
              <div class="stat-badge active-stat">
                <span class="dot active-dot"></span>
                {{ getActiveCount() }} Active
              </div>
              <div class="stat-badge offline-stat">
                <span class="dot offline-dot"></span>
                {{ getOfflineCount() }} Offline
              </div>
            </div>
          </div>
          
          <div class="view-controls">
            <button 
              class="view-btn" 
              [class.active]="viewMode() === 'grid'"
              (click)="setViewMode('grid')"
              title="Grid View"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect width="7" height="7" x="3" y="3" rx="1"/><rect width="7" height="7" x="14" y="3" rx="1"/><rect width="7" height="7" x="14" y="14" rx="1"/><rect width="7" height="7" x="3" y="14" rx="1"/></svg>
            </button>
            <button 
              class="view-btn" 
              [class.active]="viewMode() === 'table'"
              (click)="setViewMode('table')"
              title="Table View"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><line x1="8" x2="21" y1="6" y2="6"/><line x1="8" x2="21" y1="12" y2="12"/><line x1="8" x2="21" y1="18" y2="18"/><line x1="3" x2="3.01" y1="6" y2="6"/><line x1="3" x2="3.01" y1="12" y2="12"/><line x1="3" x2="3.01" y1="18" y2="18"/></svg>
            </button>
          </div>
        </div>

        @if (isLoading()) {
          <div class="loading-state">
            <div class="spinner"></div>
            <p>Loading users...</p>
          </div>
        } @else if (error()) {
          <div class="error-state">
            <p>{{ error() }}</p>
            <button (click)="loadUsers()" class="retry-btn">Retry</button>
          </div>
        } @else if (filteredUsers().length === 0) {
          <div class="empty-state">
            <p>No users found for this selection.</p>
          </div>
        } @else {
          <!-- Grid View -->
          <div class="users-grid" *ngIf="viewMode() === 'grid'">
            @for (user of filteredUsers(); track user.id) {
              <div class="user-card" [class.offline-card]="user.status === 'offline'">
                <div class="card-header">
                  <div class="user-identity">
                    <div class="avatar">{{ user.name.charAt(0) }}</div>
                    <div>
                      <h3>{{ user.name }}</h3>
                      <span class="status-text" [class]="user.status">
                        <span class="dot" [class]="user.status + '-dot'"></span>
                        {{ user.status === 'active' ? 'Reachable' : 'Unable to connect' }}
                      </span>
                    </div>
                  </div>
                  <div class="tag">{{ user.ailTag }}</div>
                </div>
                
                <div class="card-body">
                  <div class="info-row">
                    <span class="label">Project</span>
                    <span class="value">{{ user.project }}</span>
                  </div>
                  <div class="info-row">
                    <span class="label">IP Address</span>
                    <span class="value IP-code copyable" (click)="copyToClipboard($event, user.ipAddress)" title="Click to copy">{{ user.ipAddress }}</span>
                  </div>
                  <div class="info-row">
                    <span class="label">Password</span>
                    <span class="value code-val copyable" (click)="copyToClipboard($event, user.password)" title="Click to copy">{{ user.password }}</span>
                  </div>
                  <div class="info-row">
                    <span class="label">GRUB</span>
                    <span class="value code-val copyable" (click)="copyToClipboard($event, user.grubPassword)" title="Click to copy">{{ user.grubPassword }}</span>
                  </div>
                  <div class="info-row">
                    <span class="label">Last Connected</span>
                    <span class="value">{{ formatLastConnected(user.lastConnected) }}</span>
                  </div>
                </div>
              </div>
            }
          </div>

          <!-- Table View -->
          <div class="table-container" *ngIf="viewMode() === 'table'">
            <table class="users-table">
              <thead>
                <tr>
                  <th>User</th>
                  <th>Status</th>
                  <th>Project</th>
                  <th>IP Address</th>
                  <th>Password</th>
                  <th>GRUB Password</th>
                  <th>AIL Tag</th>
                  <th>Last Connected</th>
                </tr>
              </thead>
              <tbody>
                @for (user of filteredUsers(); track user.id) {
                  <tr>
                    <td>
                      <div class="user-identity-table">
                        <div class="avatar-small" [class.offline-avatar]="user.status === 'offline'">
                          {{ user.name.charAt(0) }}
                        </div>
                        <span class="user-name-cell">{{ user.name }}</span>
                      </div>
                    </td>
                    <td>
                      <span class="status-badge-table" [class]="'badge-' + user.status">
                        <span class="dot" [class]="user.status + '-dot'"></span>
                        {{ user.status === 'active' ? 'Reachable' : 'Offline' }}
                      </span>
                    </td>
                    <td class="project-cell">{{ user.project }}</td>
                    <td class="IP-code copyable" (click)="copyToClipboard($event, user.ipAddress)" title="Click to copy">{{ user.ipAddress }}</td>
                    <td class="code-val copyable" (click)="copyToClipboard($event, user.password)" title="Click to copy">{{ user.password }}</td>
                    <td class="code-val copyable" (click)="copyToClipboard($event, user.grubPassword)" title="Click to copy">{{ user.grubPassword }}</td>
                    <td><span class="tag-table">{{ user.ailTag }}</span></td>
                    <td class="date-cell">{{ formatLastConnected(user.lastConnected) }}</td>
                  </tr>
                }
              </tbody>
            </table>
          </div>
        }
      </main>
    </div>
  `,
  styles: [`
    .users-layout {
      max-width: 1280px;
      margin: 0 auto;
      display: grid;
      grid-template-columns: 260px 1fr;
      gap: 2rem;
      align-items: start;
      animation: slideUp 0.4s ease;
    }

    /* Sidebar Styles */
    .sidebar {
      background: white;
      border-radius: 1rem;
      padding: 1.5rem;
      border: 1px solid #e5e7eb;
      position: sticky;
      top: 6rem;
      box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
    }

    .sidebar h3 {
      font-size: 0.875rem;
      text-transform: uppercase;
      letter-spacing: 0.05em;
      color: #6b7280;
      margin: 0 0 1rem 0;
      font-weight: 600;
      padding-bottom: 0.75rem;
      border-bottom: 1px solid #f3f4f6;
    }

    .project-nav {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }

    .nav-item {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
      text-align: left;
      background: none;
      border: none;
      padding: 0.75rem 1rem;
      border-radius: 0.5rem;
      cursor: pointer;
      color: #4b5563;
      font-weight: 500;
      font-size: 0.95rem;
      transition: all 0.2s ease;
      font-family: inherit;
    }

    .nav-item:hover {
      background-color: #f3f4f6;
      color: #111827;
    }

    .nav-item.active {
      background: linear-gradient(135deg, #e0e7ff, #ede9fe);
      color: #4f46e5;
      font-weight: 600;
    }

    .count-badge {
      background: #f3f4f6;
      padding: 0.1rem 0.6rem;
      border-radius: 1rem;
      font-size: 0.75rem;
      color: #6b7280;
    }

    .nav-item.active .count-badge {
      background: rgba(79, 70, 229, 0.1);
      color: #4f46e5;
    }

    .project-name {
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      max-width: 150px;
    }

    /* Main Content Styles */
    .users-main {
      min-width: 0;
    }

    .users-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-end;
      margin-bottom: 2rem;
    }

    .header-left {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .view-controls {
      display: flex;
      gap: 0.5rem;
      background: #f3f4f6;
      padding: 0.25rem;
      border-radius: 0.5rem;
    }

    .view-btn {
      background: transparent;
      border: none;
      color: #6b7280;
      padding: 0.5rem;
      border-radius: 0.375rem;
      cursor: pointer;
      transition: all 0.2s;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .view-btn:hover {
      color: #111827;
      background: #e5e7eb;
    }

    .view-btn.active {
      background: white;
      color: #4f46e5;
      box-shadow: 0 1px 3px rgba(0,0,0,0.1);
    }

    h2 {
      font-size: 2rem;
      font-weight: 700;
      color: #111827;
      margin: 0;
    }

    .stats {
      display: flex;
      gap: 1rem;
    }

    .stat-badge {
      display: flex;
      align-items: center;
      gap: 0.375rem;
      padding: 0.5rem 1rem;
      border-radius: 2rem;
      font-size: 0.875rem;
      font-weight: 600;
    }

    .active-stat { background: #dcfce7; color: #166534; }
    .offline-stat { background: #fee2e2; color: #991b1b; }

    /* Grid Styles */
    .users-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 1.5rem;
    }

    .empty-state, .loading-state, .error-state {
      grid-column: 1 / -1;
      text-align: center;
      padding: 4rem;
      background: white;
      border-radius: 1rem;
      border: 1px dashed #d1d5db;
      color: #6b7280;
    }
    
    .loading-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 1rem;
    }
    
    .spinner {
      border: 3px solid rgba(79, 70, 229, 0.1);
      border-radius: 50%;
      border-top: 3px solid #4f46e5;
      width: 2rem;
      height: 2rem;
      animation: spin 1s linear infinite;
    }

    .error-state {
      color: #ef4444;
      border-color: #fca5a5;
      background: #fef2f2;
    }
    
    .retry-btn {
      margin-top: 1rem;
      background: #ef4444;
      color: white;
      border: none;
      padding: 0.5rem 1rem;
      border-radius: 0.25rem;
      cursor: pointer;
      font-weight: 500;
    }

    .user-card {
      background: white;
      border: 1px solid #e5e7eb;
      border-radius: 1rem;
      padding: 1.5rem;
      box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .user-card:hover {
      transform: translateY(-4px);
      box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
    }

    .offline-card {
      background: #fdfbfb;
      border-color: #f3f4f6;
    }

    .card-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 1.5rem;
      padding-bottom: 1rem;
      border-bottom: 1px solid #f3f4f6;
    }

    .user-identity {
      display: flex;
      align-items: center;
      gap: 1rem;
    }

    .avatar {
      width: 3rem;
      height: 3rem;
      border-radius: 50%;
      background: linear-gradient(135deg, #6366f1, #a855f7);
      color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.25rem;
      font-weight: 700;
      box-shadow: 0 2px 4px rgba(99, 102, 241, 0.3);
    }

    .offline-card .avatar, .offline-avatar {
      background: linear-gradient(135deg, #9ca3af, #d1d5db);
      box-shadow: none;
    }

    h3 {
      margin: 0 0 0.25rem 0;
      font-size: 1.125rem;
      color: #111827;
    }

    .status-text {
      display: flex;
      align-items: center;
      gap: 0.375rem;
      font-size: 0.8125rem;
      font-weight: 500;
    }

    .active { color: #059669; }
    .offline { color: #dc2626; }

    .dot {
      width: 8px;
      height: 8px;
      border-radius: 50%;
      display: inline-block;
    }

    .active-dot { background-color: #10b981; box-shadow: 0 0 8px rgba(16, 185, 129, 0.6); }
    .offline-dot { background-color: #ef4444; }

    .tag, .tag-table {
      background: #f3f4f6;
      color: #4b5563;
      padding: 0.25rem 0.625rem;
      border-radius: 0.375rem;
      font-size: 0.75rem;
      font-weight: 600;
      letter-spacing: 0.05em;
      white-space: nowrap;
    }

    .card-body {
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
    }

    .info-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      font-size: 0.9rem;
    }

    .label {
      color: #6b7280;
      font-weight: 500;
    }

    .value {
      color: #374151;
      font-weight: 600;
    }

    .IP-code {
      font-family: 'JetBrains Mono', monospace;
      font-size: 0.85rem;
      color: #4338ca;
      letter-spacing: 0.025em;
    }
    
    .code-val {
      font-family: 'JetBrains Mono', monospace;
      font-size: 0.85rem;
      color: #64748b;
    }

    .copyable {
      cursor: pointer;
      padding: 0.2rem 0.4rem;
      margin: -0.2rem -0.4rem;
      border-radius: 0.25rem;
      transition: background-color 0.2s, color 0.2s;
    }

    .copyable:hover {
      background-color: #f1f5f9;
      color: #0f172a;
    }

    td.copyable:hover {
      background-color: transparent;
      text-decoration: underline;
    }

    /* Table Styles */
    .table-container {
      background: white;
      border-radius: 1rem;
      border: 1px solid #e5e7eb;
      overflow: hidden;
      box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
      animation: fadeIn 0.3s ease;
    }

    .users-table {
      width: 100%;
      border-collapse: collapse;
      text-align: left;
    }

    .users-table th {
      background: #f9fafb;
      padding: 1rem 1.5rem;
      font-size: 0.85rem;
      font-weight: 600;
      color: #6b7280;
      text-transform: uppercase;
      letter-spacing: 0.05em;
      border-bottom: 1px solid #e5e7eb;
    }

    .users-table td {
      padding: 1rem 1.5rem;
      border-bottom: 1px solid #e5e7eb;
      vertical-align: middle;
    }

    .users-table tr:last-child td {
      border-bottom: none;
    }

    .users-table tr:hover {
      background-color: #f8fafc;
    }

    .user-identity-table {
      display: flex;
      align-items: center;
      gap: 1rem;
    }

    .avatar-small {
      width: 2.5rem;
      height: 2.5rem;
      border-radius: 50%;
      background: linear-gradient(135deg, #6366f1, #a855f7);
      color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.1rem;
      font-weight: 700;
      box-shadow: 0 2px 4px rgba(99, 102, 241, 0.2);
    }

    .user-name-cell {
      font-weight: 600;
      color: #111827;
      font-size: 0.95rem;
    }

    .status-badge-table {
      display: inline-flex;
      align-items: center;
      gap: 0.4rem;
      padding: 0.35rem 0.75rem;
      border-radius: 2rem;
      font-size: 0.85rem;
      font-weight: 600;
    }

    .badge-active {
      background: #ecfdf5;
      color: #059669;
    }

    .badge-offline {
      background: #fef2f2;
      color: #dc2626;
    }

    .project-cell {
      font-weight: 500;
      color: #4b5563;
    }

    .date-cell {
      color: #6b7280;
      font-size: 0.9rem;
    }

    @media (max-width: 1024px) {
      .users-table {
        display: block;
        overflow-x: auto;
      }
    }

    @media (max-width: 768px) {
      .users-layout {
        grid-template-columns: 1fr;
      }
      
      .sidebar {
        position: static;
      }

      .users-header {
        flex-direction: column;
        align-items: flex-start;
        gap: 1rem;
      }
    }

    @keyframes slideUp {
      from { opacity: 0; transform: translateY(15px); }
      to { opacity: 1; transform: translateY(0); }
    }

    @keyframes fadeIn {
      from { opacity: 0; }
      to { opacity: 1; }
    }
    
    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }
  `]
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
    return currentUsers.filter(u => u.project === project);
  });

  uniqueProjects = computed(() => {
    return Array.from(new Set(this.users().map(u => u.project))).sort();
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
        const mappedUsers = apiUsers.map(apiUser => {
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
            ailTag: 'AIL-' + apiUser.id.substring(0, 3).toUpperCase(), // Mocked tag using ID
            status: hoursDifference > inactiveThresholdHours ? 'offline' : 'active',
            lastConnected: apiUser.lastConnected
          } as User;
        });

        this.users.set(mappedUsers);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load users', err);
        // Fallback to dummy data if local API fails specifically for the demo
        this.isLoading.set(false);
        this.error.set('Failed to connect to the backend server at localhost:5257. Make sure the API is running.');
      }
    });
  }

  setProject(project: string) {
    this.selectedProject.set(project);
  }

  setViewMode(mode: ViewMode) {
    this.viewMode.set(mode);
  }

  getProjectCount(project: string): number {
    return this.users().filter(u => u.project === project).length;
  }

  getActiveCount(): number {
    return this.filteredUsers().filter(u => u.status === 'active').length;
  }

  getOfflineCount(): number {
    return this.filteredUsers().filter(u => u.status === 'offline').length;
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
    navigator.clipboard.writeText(text).then(() => {
      const target = event.target as HTMLElement;
      const originalText = target.innerText;
      target.innerText = 'Copied!';
      target.style.color = '#10b981'; // Success green text
      setTimeout(() => {
        target.innerText = originalText;
        target.style.color = ''; // Revert to default
      }, 1500);
    }).catch(err => {
      console.error('Failed to copy text: ', err);
    });
  }
}

