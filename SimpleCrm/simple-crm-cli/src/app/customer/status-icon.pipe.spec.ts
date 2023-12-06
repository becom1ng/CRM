import { StatusIconPipe } from './status-icon.pipe';

describe('StatusIconPipe', () => {
  it('create an instance', () => {
    const pipe = new StatusIconPipe();
    expect(pipe).toBeTruthy();
  });
  it('Prospect should result in online', () => {
    const pipe = new StatusIconPipe(); // 1. SETUP: construct a new instance of the class.
    const x = pipe.transform('Prospect'); // 2. INVOKE the method
    expect(x).toEqual('question'); // 3. VERIFY the result of the method matches what is expected.
  });
  it('prospect (lowercase) should result in online', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('prospect');
    expect(x).toEqual('question');
  });
  it('prOspEct (mixed case) should result in online', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('prOspEct');
    expect(x).toEqual('question');
  });
  it('Purchased should result in shield', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('Purchased');
    expect(x).toEqual('shield');
  });
  it('purchased should result in shield', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('purchased');
    expect(x).toEqual('shield');
  });
  it('pUrchased should result in shield', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('pUrchased');
    expect(x).toEqual('shield');
  });
  it('(empty string) should result in user', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('');
    expect(x).toEqual('user');
  });
  it('null should result in user', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform(null);
    expect(x).toEqual('user');
  });
});
