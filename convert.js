var data = `1
00:00:00,539 --> 00:00:03,350

hey guys what's up oh my gosh so I'm

2
00:00:03,350 --> 00:00:03,360
hey guys what's up oh my gosh so I'm
 

3
00:00:03,360 --> 00:00:05,030
hey guys what's up oh my gosh so I'm
super excited we just did the exact

4
00:00:05,030 --> 00:00:05,040
super excited we just did the exact
 

5
00:00:05,040 --> 00:00:08,509
super excited we just did the exact
thing show and um near it they had these

6
00:00:08,509 --> 00:00:08,519
thing show and um near it they had these
 

7
00:00:08,519 --> 00:00:10,490
thing show and um near it they had these
little cheesy corn dogs thing we're

8
00:00:10,490 --> 00:00:10,500
little cheesy corn dogs thing we're
 

9
00:00:10,500 --> 00:00:11,629
little cheesy corn dogs thing we're
about to go do one more thing in this

10
00:00:11,629 --> 00:00:11,639
about to go do one more thing in this
 

11
00:00:11,639 --> 00:00:13,669
about to go do one more thing in this
area and I was like super excited

12
00:00:13,669 --> 00:00:13,679
area and I was like super excited
 

13
00:00:13,679 --> 00:00:15,230
area and I was like super excited
because I haven't been so long I've had

14
00:00:15,230 --> 00:00:15,240
because I haven't been so long I've had
 

15
00:00:15,240 --> 00:00:16,910
because I haven't been so long I've had
one of these in like what like it's been

16
00:00:16,910 --> 00:00:16,920
one of these in like what like it's been
 

17
00:00:16,920 --> 00:00:18,950
one of these in like what like it's been
I mean it's crazy that's so long time

18
00:00:18,950 --> 00:00:18,960
I mean it's crazy that's so long time
 

19
00:00:18,960 --> 00:00:20,689
I mean it's crazy that's so long time
like I mean I used to love these things

20
00:00:20,689 --> 00:00:20,699
like I mean I used to love these things
 

21
00:00:20,699 --> 00:00:22,189
like I mean I used to love these things
and someone was eating in the other day

22
00:00:22,189 --> 00:00:22,199
and someone was eating in the other day
 

23
00:00:22,199 --> 00:00:24,590
and someone was eating in the other day
and I was like I really miss these

24
00:00:24,590 --> 00:00:24,600
and I was like I really miss these
 

25
00:00:24,600 --> 00:00:26,330
and I was like I really miss these
oh you did get something with Cheetos on

26
00:00:26,330 --> 00:00:26,340
oh you did get something with Cheetos on
 

27
00:00:26,340 --> 00:00:27,589
oh you did get something with Cheetos on
it that's someone like you that's their

28
00:00:27,589 --> 00:00:27,599
it that's someone like you that's their
 

29
00:00:27,599 --> 00:00:29,029
it that's someone like you that's their
fries I mean I figured because we didn't

30
00:00:29,029 --> 00:00:29,039
fries I mean I figured because we didn't
 

31
00:00:29,039 --> 00:00:31,730
fries I mean I figured because we didn't
get any on the uh I was like okay let's

32
00:00:31,730 --> 00:00:31,740
get any on the uh I was like okay let's
 

33
00:00:31,740 --> 00:00:32,990
get any on the uh I was like okay let's
see what

34
00:00:32,990 --> 00:00:33,000
see what
 
`;


var subs_json = data.split("\n\n")
  .map(item => {
    var parts = item.split("\n");
  if (!parts[1]) return null;
  var ts_norm=parts[1].split(',')[0];
    var start = ts_norm.split(':');
    var start_ts = parseInt(start[0] * 3600) + parseInt(start[1] * 60) + parseInt(start[2]);

    return {
      number: parts[0],
      timestamp: ts_norm,
      seconds: start_ts,
      text: parts[2],
    };
  })
  .filter(ob => ob?.text);

console.log(subs_json);