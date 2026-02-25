import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

interface UserStatus {
    id: number;
    name: string;
    avatar: string;
}

@Component({
    selector: 'app-task-details',
    standalone: true,
    imports: [CommonModule, RouterLink],
    template: `
    <div class="task-details-container">
      <div class="header-actions">
        <a routerLink="/tasks" class="back-link">
          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="m15 18-6-6 6-6"/></svg>
          Back to Tasks
        </a>
      </div>

      <div class="task-header">
        <div class="title-section">
          <h2>{{ taskTitle }}</h2>
          <span class="project-tag">{{ taskProject }}</span>
        </div>
        <button class="run-btn">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="currentColor" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polygon points="5 3 19 12 5 21 5 3"/></svg>
          Execute Script
        </button>
      </div>

      <div class="script-section">
        <div class="section-header">
          <h3>Execution Script</h3>
          <span class="lang-badge">bash</span>
        </div>
        <pre class="code-block"><code>{{ scriptContent }}</code></pre>
      </div>

      <div class="users-deployment-section">
        <h3 class="section-title">Deployment Status</h3>
        
        <div class="columns-grid">
          <!-- Pending Column -->
          <div class="status-column pending-col">
            <div class="col-header">
              <div class="col-title">
                <div class="status-dot pending-dot"></div>
                <h4>Pending Execution</h4>
              </div>
              <span class="count-badge">{{ pendingUsers.length }}</span>
            </div>
            
            <div class="user-list">
              @for (user of pendingUsers; track user.id) {
                <div class="compact-user-card">
                  <div class="avatar">{{ user.avatar }}</div>
                  <span class="user-name">{{ user.name }}</span>
                  <button class="action-btn" title="Run task for this user">Run</button>
                </div>
              }
            </div>
          </div>

          <!-- Completed Column -->
          <div class="status-column completed-col">
            <div class="col-header">
              <div class="col-title">
                <div class="status-dot completed-dot"></div>
                <h4>Completed successfully</h4>
              </div>
              <span class="count-badge completed-badge">{{ completedUsers.length }}</span>
            </div>
            
            <div class="user-list">
              @for (user of completedUsers; track user.id) {
                <div class="compact-user-card completed-card">
                  <div class="avatar completed-avatar">{{ user.avatar }}</div>
                  <span class="user-name">{{ user.name }}</span>
                  <svg class="check-icon" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"/></svg>
                </div>
              }
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
    styles: [`
    .task-details-container {
      max-width: 1000px;
      margin: 0 auto;
      animation: fadeIn 0.3s ease;
    }

    .header-actions {
      margin-bottom: 2rem;
    }

    .back-link {
      display: inline-flex;
      align-items: center;
      gap: 0.25rem;
      color: #6b7280;
      text-decoration: none;
      font-weight: 500;
      font-size: 0.95rem;
      transition: color 0.2s;
    }

    .back-link:hover {
      color: #111827;
    }

    .task-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 2.5rem;
      padding-bottom: 1.5rem;
      border-bottom: 1px solid #f3f4f6;
    }

    .title-section h2 {
      font-size: 2.25rem;
      font-weight: 800;
      color: #111827;
      margin: 0 0 0.5rem 0;
      letter-spacing: -0.02em;
    }

    .project-tag {
      display: inline-block;
      background: linear-gradient(135deg, #e0e7ff, #ede9fe);
      color: #4f46e5;
      padding: 0.3rem 0.75rem;
      border-radius: 2rem;
      font-size: 0.85rem;
      font-weight: 600;
      letter-spacing: 0.05em;
    }

    .run-btn {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      background: #111827;
      color: white;
      border: none;
      padding: 0.75rem 1.5rem;
      border-radius: 0.5rem;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.2s;
    }

    .run-btn:hover {
      background: #374151;
      transform: translateY(-1px);
    }

    .script-section {
      background: #1c1917;
      border-radius: 1rem;
      overflow: hidden;
      margin-bottom: 3rem;
      box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
    }

    .section-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      background: #292524;
      padding: 0.75rem 1.25rem;
      border-bottom: 1px solid #44403c;
    }

    .section-header h3 {
      color: #e7e5e4;
      margin: 0;
      font-size: 0.9rem;
      font-weight: 500;
    }

    .lang-badge {
      color: #a8a29e;
      font-size: 0.75rem;
      font-family: monospace;
      background: #44403c;
      padding: 0.1rem 0.5rem;
      border-radius: 0.25rem;
    }

    .code-block {
      margin: 0;
      padding: 1.5rem;
      color: #f8fafc;
      font-family: 'JetBrains Mono', 'Fira Code', Consolas, monospace;
      font-size: 0.9rem;
      line-height: 1.6;
      overflow-x: auto;
    }

    .section-title {
      font-size: 1.5rem;
      margin: 0 0 1.5rem 0;
    }

    .columns-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 2rem;
    }

    .status-column {
      background: white;
      border-radius: 1rem;
      padding: 1.5rem;
      border: 1px solid #e5e7eb;
      min-height: 400px;
    }

    .pending-col {
      background: #fafaf9;
    }

    .col-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 1.5rem;
      padding-bottom: 1rem;
      border-bottom: 1px solid #e5e7eb;
    }

    .col-title {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .col-title h4 {
      margin: 0;
      font-size: 1.1rem;
      color: #1f2937;
    }

    .status-dot {
      width: 10px;
      height: 10px;
      border-radius: 50%;
    }

    .pending-dot { background: #f59e0b; }
    .completed-dot { background: #10b981; }

    .count-badge {
      background: #f1f5f9;
      color: #475569;
      padding: 0.25rem 0.75rem;
      border-radius: 1rem;
      font-size: 0.85rem;
      font-weight: 600;
    }

    .completed-badge {
      background: #dcfce7;
      color: #166534;
    }

    .user-list {
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
    }

    .compact-user-card {
      display: flex;
      align-items: center;
      gap: 1rem;
      padding: 0.75rem;
      background: white;
      border: 1px solid #e5e7eb;
      border-radius: 0.75rem;
      box-shadow: 0 1px 2px rgba(0,0,0,0.02);
      transition: box-shadow 0.2s;
    }

    .compact-user-card:hover {
      box-shadow: 0 4px 6px rgba(0,0,0,0.05);
    }

    .avatar {
      width: 2rem;
      height: 2rem;
      border-radius: 50%;
      background: linear-gradient(135deg, #6366f1, #a855f7);
      color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 0.85rem;
      font-weight: 700;
    }

    .completed-avatar {
      background: linear-gradient(135deg, #10b981, #059669);
    }

    .user-name {
      flex: 1;
      font-weight: 500;
      color: #374151;
      font-size: 0.95rem;
    }

    .action-btn {
      background: white;
      border: 1px solid #cbd5e1;
      padding: 0.35rem 0.75rem;
      border-radius: 0.375rem;
      font-size: 0.75rem;
      font-weight: 600;
      color: #475569;
      cursor: pointer;
      transition: all 0.2s;
    }

    .action-btn:hover {
      background: #f8fafc;
      color: #0f172a;
      border-color: #94a3b8;
    }

    .check-icon {
      color: #10b981;
      margin-right: 0.5rem;
    }

    @media (max-width: 768px) {
      .columns-grid {
        grid-template-columns: 1fr;
      }
    }

    @keyframes fadeIn {
      from { opacity: 0; transform: translateY(10px); }
      to { opacity: 1; transform: translateY(0); }
    }
  `]
})
export class TaskDetailsComponent implements OnInit {
    taskId: string | null = null;
    taskTitle: string = 'Update Salt Minion Configuration';
    taskProject: string = 'ALL';

    scriptContent: string = `#!/bin/bash
# Fetch the latest salt-minion config from the master repo
echo "Fetching latest minion config..."
curl -sL https://internal-repo.corp/salt/minion.conf -o /etc/salt/minion

# Restart the service to apply changes
echo "Restarting salt-minion service..."
systemctl restart salt-minion

# Verify status
if systemctl is-active --quiet salt-minion; then
    echo "Configuration applied successfully."
    exit 0
else
    echo "Failed to restart minion service."
    exit 1
fi`;

    pendingUsers: UserStatus[] = [
        { id: 4, name: 'Emily Davis', avatar: 'E' },
        { id: 5, name: 'James Wilson', avatar: 'J' },
        { id: 6, name: 'Daniel Taylor', avatar: 'D' }
    ];

    completedUsers: UserStatus[] = [
        { id: 1, name: 'Alex Johnson', avatar: 'A' },
        { id: 2, name: 'Sarah Williams', avatar: 'S' },
        { id: 3, name: 'Michael Chen', avatar: 'M' }
    ];

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        this.taskId = this.route.snapshot.paramMap.get('id');
        // In a real app, we would fetch the task details and user lists based on this ID
        if (this.taskId === '2') {
            this.taskTitle = 'Restart Nginx Service';
            this.taskProject = 'Frontend Dash';
            this.scriptContent = `#!/bin/bash\nsystemctl restart nginx\nsystemctl status nginx`;
        }
    }
}
