import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommandGenerator } from './command-generator';

describe('CommandGenerator', () => {
  let component: CommandGenerator;
  let fixture: ComponentFixture<CommandGenerator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommandGenerator]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommandGenerator);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
